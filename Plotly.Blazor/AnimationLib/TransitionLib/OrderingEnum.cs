/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.AnimationLib.TransitionLib
{
    /// <summary>
    ///     Determines whether the figure&#39;s layout or traces smoothly transitions
    ///     during updates that make both traces and layout change.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum OrderingEnum
    {
        [EnumMember(Value=@"layout first")]
        LayoutFirst = 0,
        [EnumMember(Value=@"traces first")]
        TracesFirst
    }
}