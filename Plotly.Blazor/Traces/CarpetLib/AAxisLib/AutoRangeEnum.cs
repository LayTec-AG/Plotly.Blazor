/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.CarpetLib.AAxisLib
{
    /// <summary>
    ///     Determines whether or not the range of this axis is computed in relation
    ///     to the input data. See <c>rangemode</c> for more info. If <c>range</c> is
    ///     provided, then <c>autorange</c> is set to <c>false</c>.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum AutoRangeEnum
    {
        [EnumMember(Value=@"true")]
        True = 0,
        [EnumMember(Value=@"False")]
        False,
        [EnumMember(Value=@"reversed")]
        Reversed
    }
}