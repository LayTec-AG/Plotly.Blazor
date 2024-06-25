/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.ScatterSmithLib.MarkerLib
{
    /// <summary>
    ///     Has an effect only if <c>marker.size</c> is set to a numerical array. Sets
    ///     the rule for which the data in <c>size</c> is converted to pixels.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum SizeModeEnum
    {
        [EnumMember(Value=@"diameter")]
        Diameter = 0,
        [EnumMember(Value=@"area")]
        Area
    }
}