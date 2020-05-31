using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Plotly.Blazor
{
    public class SubplotConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return !typeToConvert.IsPrimitive && !typeToConvert.IsEnum && typeToConvert != typeof(string);
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter) Activator.CreateInstance(
                typeof(SubplotConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { },
                null);
        }
    }

    public class SubplotConverter<T> : JsonConverter<T>
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

                if (property.GetCustomAttribute(typeof(SubplotAttribute)) == null)
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