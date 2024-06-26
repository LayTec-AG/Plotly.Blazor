/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.IcicleLib.TilingLib
{
    /// <summary>
    ///     When set in conjunction with <c>tiling.flip</c>, determines on which side
    ///     the root nodes are drawn in the chart. If <c>tiling.orientation</c> is <c>v</c>
    ///     and <c>tiling.flip</c> is *&#39;, the root nodes appear at the top. If <c>tiling.orientation</c>
    ///     is <c>v</c> and <c>tiling.flip</c> is <c>y</c>, the root nodes appear at
    ///     the bottom. If <c>tiling.orientation</c> is <c>h</c> and <c>tiling.flip</c>
    ///     is &#39;*, the root nodes appear at the left. If <c>tiling.orientation</c>
    ///     is <c>h</c> and <c>tiling.flip</c> is <c>x</c>, the root nodes appear at
    ///     the right.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum OrientationEnum
    {
        [EnumMember(Value=@"h")]
        H = 0,
        [EnumMember(Value=@"v")]
        V
    }
}