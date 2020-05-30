using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;


namespace Plotly.Blazor
{
    public static class Extensions
    {
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
                return asJsonElement.PrepareJsonElement();
            }

            // Handle objects if its not an enumerable
            if (!(obj is IEnumerable<dynamic> asEnumerable))
            {
                return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize<object>(obj, serializerOptions))
                    .PrepareJsonElement();
            }

            // Handle dictionaries
            if (asEnumerable is Dictionary<dynamic, dynamic> dictionary)
            {
                return dictionary.ToDictionary(keyValue => keyValue.Key,
                    keyValue => PrepareJsInterop(keyValue.Value, serializerOptions));
            }

            // Handle lists
            return asEnumerable.Where(item => !serializerOptions.IgnoreNullValues || item != null)
                .Select(item => PrepareJsInterop(item, serializerOptions));
        }


        private static dynamic PrepareJsonElement(this JsonElement obj)
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
    }
}