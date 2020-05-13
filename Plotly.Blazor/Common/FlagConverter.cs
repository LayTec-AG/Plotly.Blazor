using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
// ReSharper disable NotAccessedField.Local

namespace Plotly.Blazor.Common
{

    /// <summary>
    /// Class FlagConverter.
    /// Implements the <see cref="System.Text.Json.Serialization.JsonConverterFactory" />
    /// </summary>
    /// <seealso cref="System.Text.Json.Serialization.JsonConverterFactory" />
    public class FlagConverter : JsonConverterFactory
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the converter instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum && typeToConvert.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }

        /// <summary>
        /// Creates a converter for a specified type.
        /// </summary>
        /// <param name="typeToConvert">The type handled by the converter.</param>
        /// <param name="options">The serialization options to use.</param>
        /// <returns>A converter for which <typeparamref name="T" /> is compatible with <paramref name="typeToConvert" />.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(FlagConverterInner<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { options },
                null);

            return converter;
        }

        /// <summary>
        /// Class FlagConverterInner.
        /// Implements the <see cref="System.Text.Json.Serialization.JsonConverter{T}" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <seealso cref="System.Text.Json.Serialization.JsonConverter{T}" />
        private class FlagConverterInner<T> : JsonConverter<T> where T : struct, Enum
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
            private readonly JsonConverter<T> valueConverter;
            private readonly Type valueType;

            /// <summary>
            /// Initializes a new instance of the <see cref="FlagConverterInner{T}"/> class.
            /// </summary>
            /// <param name="options">The options.</param>
            public FlagConverterInner(JsonSerializerOptions options)
            {
                // For performance, use the existing converter if available.
                valueConverter = (JsonConverter<T>)options
                    .GetConverter(typeof(T));

                // Cache the key and value types.
                valueType = typeof(T);
            }

            /// <summary>
            /// Reads and converts the JSON to type <typeparamref name="T" />.
            /// </summary>
            /// <param name="reader">The reader.</param>
            /// <param name="typeToConvert">The type to convert.</param>
            /// <param name="options">An object that specifies serialization options to use.</param>
            /// <returns>The converted value.</returns>
            public override T Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                var propertyValue = reader.GetString();
                // Get the enums
                var enums = propertyValue?.Split('+')
                    .Select(e =>
                        Enum.TryParse(e, false, out T value) // Improve performance
                        || Enum.TryParse(e, true, out value)
                            ? (int)(object)value
                            : throw new JsonException($"Unable to convert \"{e}\" to Enum \"{valueType}\".")
                    );

                // Return default value, when value was null
                if (enums == null)
                {
                    return (T)(object)0;
                }

                // Set the mask
                var result = 0;
                foreach (var e in enums)
                {
                    result |= e;
                }

                // Cast mask to enum type
                // ReSharper disable once PossibleInvalidCastException
                return (T)(object)result;
            }

            /// <summary>
            /// Writes a specified value as JSON.
            /// </summary>
            /// <param name="writer">The writer to write to.</param>
            /// <param name="value">The value to convert to JSON.</param>
            /// <param name="options">An object that specifies serialization options to use.</param>
            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.GetComposition().ToLower());
            }
        }
    }
}