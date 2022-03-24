using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace Plotly.Blazor
{
    /// <summary>
    ///     Determines the format an image. 
    ///     Formats like 'EPS', 'SVG' and 'PDF' are not supported, they would require a personal or professional subscription.
    /// </summary>
    [JsonConverter(typeof(EnumConverter))]
    public enum ImageFormat
    {
        [EnumMember(Value = @"jpg")]
        Jpg,
        [EnumMember(Value = @"jpeg")]
        Jpeg,
        [EnumMember(Value = @"png")]
        Png,
    }
}
