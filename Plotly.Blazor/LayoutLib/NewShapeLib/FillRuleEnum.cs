/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Plotly.Blazor.LayoutLib.NewShapeLib
{
    /// <summary>
    ///     Determines the path&#39;s interior. For more info please visit https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/fill-rule
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    [JsonConverter(typeof(EnumConverter))]
    public enum FillRuleEnum
    {
        [EnumMember(Value=@"evenodd")]
        EvenOdd = 0,
        [EnumMember(Value=@"nonzero")]
        Nonzero
    }
}