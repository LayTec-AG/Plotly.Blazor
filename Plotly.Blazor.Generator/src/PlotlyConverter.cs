using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor
{
    public class PlotlyConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return !typeToConvert.IsPrimitive && !typeToConvert.IsEnum && typeToConvert != typeof(string);
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
                typeof(PlotlyConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { },
                null);
        }
    }

    public class PlotlyConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var properties = type.GetProperties().ToArray();

            writer.WriteStartObject();

            foreach (var property in properties)
            {
                string propertyName;
                var propertyValue = type.GetProperty(property.Name)?.GetValue(value, null);

                var nameAttributeValue = property.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name;
                if (options.PropertyNamingPolicy == null)
                {
                    propertyName = nameAttributeValue ?? property.Name;
                }
                else
                {
                    propertyName = options.PropertyNamingPolicy.ConvertName(nameAttributeValue ?? property.Name);
                }

                var containsSubplotAttr = property.GetCustomAttribute(typeof(SubplotAttribute)) != null;
                var containsArrayAttr = property.GetCustomAttribute(typeof(ArrayAttribute)) != null;

                if (containsSubplotAttr && containsArrayAttr)
                {
                    throw new NotSupportedException($"{nameof(SubplotAttribute)} and {nameof(ArrayAttribute)} are currently not supported at the same time.");
                }

                if (!containsArrayAttr && !containsSubplotAttr)
                {
                    if (propertyValue == null)
                    {
                        if (!options.IgnoreNullValues)
                        {
                            writer.WritePropertyName(propertyName);
                            writer.WriteNullValue();
                        }
                        continue;
                    }
                    writer.WritePropertyName(propertyName);
                    JsonSerializer.Serialize(writer, propertyValue, options);
                }
                else if (containsArrayAttr)
                {
                    if (propertyValue == null && options.IgnoreNullValues)
                    {
                        continue;
                    }

                    // Get the standalone property
                    var standaloneProperty = type.GetProperty(property.Name.Replace("Array", ""));
                    if (standaloneProperty == null)
                    {
                        throw new ArgumentException($"Didn't found a matching property for array property {property.Name} ");
                    }

                    var standaloneValue = type.GetProperty(standaloneProperty.Name)?.GetValue(value, null);

                    if (standaloneValue != null)
                    {
                        continue;
                    }

                    writer.WritePropertyName(propertyName);
                    JsonSerializer.Serialize(writer, propertyValue, options);
                }
                else
                {

                    if (propertyValue == null)
                    {
                        if (!options.IgnoreNullValues)
                        {
                            writer.WriteNullValue();
                        }
                        continue;
                    }

                    if (!(propertyValue is IEnumerable asEnumerable))
                    {
                        throw new NotSupportedException(
                            $"{nameof(SubplotAttribute)} is only supported for IEnumerables.");
                    }

                    var index = 0;

                    foreach (var current in asEnumerable)
                    {
                        writer.WritePropertyName(index == 0 ? propertyName : $"{propertyName}{index + 1}");
                        JsonSerializer.Serialize(writer, current, options);
                        index++;
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}