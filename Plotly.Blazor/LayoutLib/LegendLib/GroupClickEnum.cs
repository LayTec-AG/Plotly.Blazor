/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.LayoutLib.LegendLib
{
    /// <summary>
    ///     Determines the behavior on legend group item click. <c>toggleitem</c> toggles
    ///     the visibility of the individual item clicked on the graph. <c>togglegroup</c>
    ///     toggles the visibility of all items in the same legendgroup as the item
    ///     clicked on the graph.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum GroupClickEnum
    {
        [EnumMember(Value=@"togglegroup")]
        ToggleGroup = 0,
        [EnumMember(Value=@"toggleitem")]
        ToggleItem
    }
}