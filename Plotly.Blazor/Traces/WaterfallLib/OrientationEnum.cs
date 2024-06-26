/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.WaterfallLib
{
    /// <summary>
    ///     Sets the orientation of the bars. With <c>v</c> (<c>h</c>), the value of
    ///     the each bar spans along the vertical (horizontal).
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum OrientationEnum
    {
        [EnumMember(Value=@"v")]
        V,
        [EnumMember(Value=@"h")]
        H
    }
}