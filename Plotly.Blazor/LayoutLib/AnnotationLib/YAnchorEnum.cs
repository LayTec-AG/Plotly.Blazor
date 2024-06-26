/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.LayoutLib.AnnotationLib
{
    /// <summary>
    ///     Sets the text box&#39;s vertical position anchor This anchor binds the <c>y</c>
    ///     position to the <c>top</c>, <c>middle</c> or <c>bottom</c> of the annotation.
    ///     For example, if <c>y</c> is set to 1, <c>yref</c> to <c>paper</c> and <c>yanchor</c>
    ///     to <c>top</c> then the top-most portion of the annotation lines up with
    ///     the top-most edge of the plotting area. If <c>auto</c>, the anchor is equivalent
    ///     to <c>middle</c> for data-referenced annotations or if there is an arrow,
    ///     whereas for paper-referenced with no arrow, the anchor picked corresponds
    ///     to the closest side.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum YAnchorEnum
    {
        [EnumMember(Value=@"auto")]
        Auto = 0,
        [EnumMember(Value=@"top")]
        Top,
        [EnumMember(Value=@"middle")]
        Middle,
        [EnumMember(Value=@"bottom")]
        Bottom
    }
}