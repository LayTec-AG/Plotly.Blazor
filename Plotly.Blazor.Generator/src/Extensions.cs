using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text.Json;


// ReSharper disable once CheckNamespace
namespace Plotly.Blazor
{
    public static class Extensions
    {
        /// <summary>
        ///     Converts an element into an IDictionary so that the methods of the IJsRunTime
        ///     do not affect the desired serialization.
        /// </summary>
        /// <typeparam name="T">Type of the element.</typeparam>
        /// <param name="obj">The object itself.</param>
        /// <param name="serializerOptions">Custom serialization settings.</param>
        /// <returns></returns>
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static dynamic PrepareJsInterop<T>(this T obj, JsonSerializerOptions serializerOptions = null)
        {
            var type = obj?.GetType();

            // Handle simple types
            if (obj == null || !type.IsComplex())
            {
                return obj;
            }

            // Set default serializer options if necessary
            serializerOptions ??= new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = null
            };

            // Handle jsonElements
            if (obj is JsonElement asJsonElement)
            {
                return asJsonElement.PrepareJsonElement(serializerOptions);
            }

            // Handle objects if its not an enumerable
            if (!(obj is IEnumerable<dynamic> asEnumerable))
            {
                return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize<object>(obj, serializerOptions))
                    .PrepareJsonElement(serializerOptions);
            }

            // Handle dictionaries
            if (asEnumerable is Dictionary<object, object> dictionary)
            {
                var currentValues = new Dictionary<object, object>();
                // Fill new list or dictionary
                foreach (var item in dictionary)
                {
                    currentValues.Add(item.Key, PrepareJsInterop(item.Value, serializerOptions));
                }

                return currentValues;
            }

            // Handle lists
            else
            {
                var currentValues = new List<object>();

                // Fill new list or dictionary
                foreach (var item in asEnumerable)
                {
                    if (item == null && serializerOptions.IgnoreNullValues)
                    {
                        continue;
                    }

                    currentValues.Add(PrepareJsInterop(item, serializerOptions));
                }

                return currentValues;
            }
        }


        private static dynamic PrepareJsonElement(this JsonElement obj, JsonSerializerOptions serializerOptions)
        {
            switch (obj.ValueKind)
            {
                case JsonValueKind.Object:
                    IDictionary<string, object> expando = new ExpandoObject();
                    foreach (var property in obj.EnumerateObject())
                    {
                        expando.Add(property.Name, property.Value.PrepareJsonElement(serializerOptions));
                    }

                    return expando;
                case JsonValueKind.Array:
                    var json = obj.GetRawText();
                    return JsonSerializer.Deserialize<IEnumerable<JsonElement>>(json, serializerOptions);
                case JsonValueKind.String:
                    return obj.GetString();
                case JsonValueKind.Number:
                    return obj.GetSingle();
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

        private static bool IsComplex(this Type type)
        {
            return !type.IsPrimitive && !type.IsEnum && type != typeof(string) ||
                   type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}