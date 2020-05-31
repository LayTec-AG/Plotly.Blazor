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