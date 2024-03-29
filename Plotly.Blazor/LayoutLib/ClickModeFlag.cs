/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.LayoutLib
{
    /// <summary>
    ///     Determines the mode of single click interactions. <c>event</c> is the default
    ///     value and emits the <c>plotly_click</c> event. In addition this mode emits
    ///     the <c>plotly_selected</c> event in drag modes <c>lasso</c> and <c>select</c>,
    ///     but with no event data attached (kept for compatibility reasons). The <c>select</c>
    ///     flag enables selecting single data points via click. This mode also supports
    ///     persistent selections, meaning that pressing Shift while clicking, adds
    ///     to / subtracts from an existing selection. <c>select</c> with <c>hovermode</c>:
    ///     <c>x</c> can be confusing, consider explicitly setting <c>hovermode</c>:
    ///     <c>closest</c> when using this feature. Selection events are sent accordingly
    ///     as long as <c>event</c> flag is set as well. When the <c>event</c> flag
    ///     is missing, <c>plotly_click</c> and <c>plotly_selected</c> events are not
    ///     fired.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum ClickModeFlag
    {
        [EnumMember(Value=@"none")]
        None = 0,
        [EnumMember(Value=@"event")]
        Event = 1,
        [EnumMember(Value=@"select")]
        Select = 2
    }
}