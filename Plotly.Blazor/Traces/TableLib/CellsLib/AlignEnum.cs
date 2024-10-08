/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Traces.TableLib.CellsLib
{
    /// <summary>
    ///     Sets the horizontal alignment of the <c>text</c> within the box. Has an
    ///     effect only if <c>text</c> spans two or more lines (i.e. <c>text</c> contains
    ///     one or more &lt;br&gt; HTML tags) or if an explicit width is set to override
    ///     the text width.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum AlignEnum
    {
        [EnumMember(Value=@"center")]
        Center = 0,
        [EnumMember(Value=@"left")]
        Left,
        [EnumMember(Value=@"right")]
        Right
    }
}