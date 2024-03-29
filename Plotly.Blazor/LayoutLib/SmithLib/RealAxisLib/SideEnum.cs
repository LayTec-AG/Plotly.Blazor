/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.LayoutLib.SmithLib.RealAxisLib
{
    /// <summary>
    ///     Determines on which side of real axis line the tick and tick labels appear.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum SideEnum
    {
        [EnumMember(Value=@"top")]
        Top = 0,
        [EnumMember(Value=@"bottom")]
        Bottom
    }
}