/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Plotly.Blazor.LayoutLib.ImageLib
{
    /// <summary>
    ///     Specifies which dimension of the image to constrain.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    [JsonConverter(typeof(EnumConverter))]
    public enum SizingEnum
    {
        [EnumMember(Value=@"contain")]
        Contain = 0,
        [EnumMember(Value=@"fill")]
        Fill,
        [EnumMember(Value=@"stretch")]
        Stretch
    }
}