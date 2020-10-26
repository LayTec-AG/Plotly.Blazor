using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
#pragma warning disable 1591

namespace Plotly.Blazor
{
    public static class Extensions
    {
        /// <summary>
        ///     Prepares an object for js interop operations, converting the object to a dictionary.
        ///     This operation can be customized using own serializer options.
        ///     Currently it's not possible to define serializer options for the JSRuntime directly.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="serializerOptions">Optional serializerOptions.</param>
        /// <returns></returns>
        public static object PrepareJsInterop<T>(this T obj, JsonSerializerOptions serializerOptions = null)
        {
            var type = obj?.GetType();

            // Handle simple types
            if (obj == null || type.IsPrimitive || type == typeof(string))
            {
                return obj;
            }

            // Handle jsonElements
            if (obj is JsonElement jsonElement)
            {
                return jsonElement.PrepareJsonElement();
            }

            // Set default serializer options if necessary
            serializerOptions ??= new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = null
            };

            // Handle all kind of complex objects
            return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize<object>(obj, serializerOptions))
                .PrepareJsonElement();
        }


        private static object PrepareJsonElement(this JsonElement obj)
        {
            switch (obj.ValueKind)
            {
                case JsonValueKind.Object:
                    IDictionary<string, object> expando = new ExpandoObject();
                    foreach (var property in obj.EnumerateObject())
                    {
                        expando.Add(property.Name, property.Value.PrepareJsonElement());
                    }

                    return expando;
                case JsonValueKind.Array:
                    return obj.EnumerateArray().Select(jsonElement => jsonElement.PrepareJsonElement());
                case JsonValueKind.String:
                    return obj.GetString();
                case JsonValueKind.Number:
                    return obj.GetDecimal();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.Undefined:
                    return null;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        ///     Adds data to an IList.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        ///     Updates the properties of a given object, using a json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        internal static T Populate<T>(this T obj, string jsonString) where T : class
        {
            var newObj = JsonSerializer.Deserialize(jsonString, obj.GetType());

            foreach (var property in newObj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite))
            { 
                if (!obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite).Any(x => x.Name == property.Name && property.GetValue(newObj) != default))
                {
                    continue;
                }

                if (property.GetType().IsClass && property.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)
                {
                    var mapMethod = typeof(Extensions).GetMethod("Populate");
                    if(mapMethod == null) throw new NullReferenceException(nameof(mapMethod));
                    var genericMethod = mapMethod.MakeGenericMethod(property.GetValue(newObj).GetType());
                    var obj2 = genericMethod.Invoke(null, new[] { property.GetValue(newObj), JsonSerializer.Serialize(property.GetValue(newObj).ToString()) });

                    foreach (var property2 in obj2.GetType().GetProperties())
                    {
                        if (property2.GetValue(obj2) != null)
                        {
                            property?.GetValue(obj)?.GetType()?.GetProperty(property2.Name)?.SetValue(property.GetValue(obj), property2.GetValue(obj2));
                        }
                    }
                }
                else
                {
                    property.SetValue(obj, property.GetValue(newObj));
                }
            }

            return obj;
        }

        /// <summary>
        ///     Inserts data to an IList by given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public static void InsertRange<T>(this IList<T> list, int index, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (list is List<T> asList)
            {
                asList.InsertRange(index, items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Insert(0, item);
                }
            }
        }
    }
}