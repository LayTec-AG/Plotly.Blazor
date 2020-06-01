/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Plotly.Blazor.LayoutLib.PolarLib.RadialAxisLib
{
    /// <summary>
    ///     Determines on which side of radial axis line the tick and tick labels appear.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    [JsonConverter(typeof(EnumConverter))]
    public enum SideEnum
    {
        [EnumMember(Value=@"clockwise")]
        Clockwise = 0,
        [EnumMember(Value=@"counterclockwise")]
        Counterclockwise
    }
}