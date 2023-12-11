/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.PieLib
{
    /// <summary>
    ///     Specifies the location of the <c>textinfo</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum TextPositionEnum
    {
        [EnumMember(Value=@"auto")]
        Auto = 0,
        [EnumMember(Value=@"inside")]
        Inside,
        [EnumMember(Value=@"outside")]
        Outside,
        [EnumMember(Value=@"none")]
        None
    }
}