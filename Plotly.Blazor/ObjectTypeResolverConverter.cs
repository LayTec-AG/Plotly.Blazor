using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace Plotly.Blazor
{
	/// <summary>
	/// Converter which deserializes a json type into an appropriate c# primitive type or, if complex type, then a dictionary or
	/// list of dictionaries.
	/// Booleans, nulls, strings are handled normally. Json numbers get converted to int32, int64 or decimal.
	/// Objects get converted into a <c>dictionary{string, object}</c>
	/// </summary>
	public class ObjectTypeResolverConverter : JsonConverter<object>
	{
		public override object Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return reader.TokenType switch
			{
				JsonTokenType.Number      => ConvertNumber(reader),
				JsonTokenType.String      => reader.GetString(),
				JsonTokenType.True        => true,
				JsonTokenType.False       => false,
				JsonTokenType.Null        => null,
				JsonTokenType.StartObject => JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options),
				JsonTokenType.StartArray  => JsonSerializer.Deserialize<List<object>>(ref reader, options),
				_                         => throw new JsonException($"Unexpected token type: {reader.TokenType}")
			};

			#region

			object ConvertNumber(
				Utf8JsonReader reader)
			{
				if (reader.TryGetInt32(out var integerValue))
				{
					return integerValue;
				}
				if (reader.TryGetInt64(out var longValue))
				{
					return longValue;
				}
				if (reader.TryGetDecimal(out var decimalValue))
				{
					return decimalValue;
				}
				// Can't think of any normal number that can't be converted to a decimal
				throw new NotSupportedException();
			}

			#endregion
		}

		public override void Write(
			Utf8JsonWriter writer, 
			object value, 
			JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, value.GetType(), options);
		}
	}
}
