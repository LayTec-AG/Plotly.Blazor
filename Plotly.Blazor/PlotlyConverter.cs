using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected startObject.");
            }

            var allProperties = typeToConvert.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite).ToList();

            var arrayProperties = allProperties.Where(p => p.GetCustomAttribute<ArrayAttribute>() != null).ToArray();
            var subplotProperties = allProperties.Where(p => p.GetCustomAttribute<SubplotAttribute>() != null && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)).ToArray();
            var otherProperties = allProperties.Where(p => p.GetCustomAttribute<SubplotAttribute>() == null && p.GetCustomAttribute<ArrayAttribute>() == null).ToArray();

            var result = (T)Activator.CreateInstance(typeToConvert);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected propertyName.");
                }

                var propertyName = reader.GetString();

                var arrayProperty = arrayProperties.FirstOrDefault(p =>
                    string.Equals(p.Name, $"{propertyName}Array", StringComparison.OrdinalIgnoreCase));

                var subplotProperty = subplotProperties.FirstOrDefault(p =>
                    Regex.IsMatch(propertyName, $"{p.Name}\\d*", RegexOptions.IgnoreCase));

                var otherProperty = otherProperties.FirstOrDefault(p =>
                    string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));


                // Determine if its an "array" property which could be either a single object or an array
                if (arrayProperty != null)
                {
                    // If its an array property, check if it start with an array token type
                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        var type = arrayProperty.PropertyType;
                        arrayProperty.SetValue(result, JsonSerializer.Deserialize(ref reader, type, options));
                    }
                    // Otherwise set the standalone value instead of the array property
                    else
                    {

                        var standalonePropertyName = Regex.Match(arrayProperty.Name, "(.*?)Array").Groups[1].Value;
                        var standaloneProperty =
                            otherProperties.FirstOrDefault(p => string.Equals(standalonePropertyName, p.Name, StringComparison.OrdinalIgnoreCase));
                        if (standaloneProperty == null)
                            throw new JsonException("The array property is missing its single counterpart.");

                        var type = standaloneProperty.PropertyType;
                        standaloneProperty.SetValue(result, JsonSerializer.Deserialize(ref reader, type, options));
                    }

                }
                else if (subplotProperty != null)
                {
                    // Get the current index
                    var match = Regex.Match(propertyName, $"{subplotProperty.Name}(\\d*)", RegexOptions.IgnoreCase);
                    int index;
                    index = int.TryParse(match.Groups[1].Value, out index) ? index - 1 : 0;

                    // Create a list, if a list does not exists yet
                    if (subplotProperty.GetValue(result) == null)
                    {
                        var constructedListType = typeof(List<>).MakeGenericType(subplotProperty.PropertyType.GenericTypeArguments[0]);
                        subplotProperty.SetValue(result, Activator.CreateInstance(constructedListType));
                    }

                    var list = subplotProperty.GetValue(result);
                    var underlyingType = subplotProperty.PropertyType.GetGenericArguments()[0];

                    // Fill the list with nulls, if not enough elements are present
                    while (((IList)list)?.Count < index)
                    {
                        list.GetType().GetMethod("Add")?.Invoke(list, new object[]{null});
                    }

                    list?.GetType().GetMethod("Insert")?.Invoke(list,
                        new[]
                        {
                            index,
                            JsonSerializer.Deserialize(ref reader, underlyingType, options)
                        });

                }
                else if (otherProperty != null)
                {
                    var type = otherProperty.PropertyType;
                    otherProperty.SetValue(result, JsonSerializer.Deserialize(ref reader, type, options));
                }
                else
                {
                    // ignore unknown/read-only properties
                    reader.Skip();
                }
            }

            throw new JsonException();
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
                        if (!options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
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
                    if (propertyValue == null && options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
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
                        if (!options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
                        {
                            writer.WriteNullValue();
                        }
                        continue;
                    }

                    if (propertyValue is not IEnumerable asEnumerable)
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