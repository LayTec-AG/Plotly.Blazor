/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.IndicatorLib.DeltaLib
{
    /// <summary>
    ///     Sets the position of delta with respect to the number.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum PositionEnum
    {
        [EnumMember(Value=@"bottom")]
        Bottom = 0,
        [EnumMember(Value=@"top")]
        Top,
        [EnumMember(Value=@"left")]
        Left,
        [EnumMember(Value=@"right")]
        Right
    }
}