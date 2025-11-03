using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options)
        {
            if (dateTimeValue is { Hour: 0, Minute: 0, Second: 0 })
            {
                writer.WriteStringValue(dateTimeValue.ToString(
                    "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteStringValue(dateTimeValue.ToString(
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }
    }

    public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        private const string Format =  "yyyy-MM-dd HH:mm:ss.ffffff";
        
        public override DateTimeOffset Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
            DateTimeOffset.ParseExact(reader.GetString(), Format, CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTimeOffset dateTimeValue,
            JsonSerializerOptions options)
        {
            if (dateTimeValue is { Hour: 0, Minute: 0, Second: 0 })
            {
                writer.WriteStringValue(dateTimeValue.ToString(
                    "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteStringValue(dateTimeValue.ToString(Format, CultureInfo.InvariantCulture));
            }
        }

    }
}
