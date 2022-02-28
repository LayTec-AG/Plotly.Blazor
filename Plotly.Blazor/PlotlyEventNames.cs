using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Plotly.Blazor
{
    /// <summary>
    ///    Names of Plotly events for registering with chart.
    ///    See https://plotly.com/javascript/plotlyjs-events/#event-data
    /// </summary>
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum PlotlyEventNames : long
    {
        /// <summary>
        /// No events
        /// </summary>
        None = 0,

        /// <summary>
        /// Click Event: plotly_click
        /// </summary>
        [EnumMember(Value = @"plotly_click")]
        Click = 1,

        /// <summary>
        /// Legend Click Event: plotly_legendclick
        /// </summary>
        [EnumMember(Value = @"plotly_legendclick")]
        LegendClick = 1 << 1,

        /// <summary>
        /// Legend Click Event: 
        /// </summary>
        [EnumMember(Value = @"plotly_legenddoubleclick")]
        LegendDoubleClick = 1 << 2,

        /// <summary>
        /// Hover Event: plotly_hover
        /// </summary>
        [EnumMember(Value = @"plotly_hover")]
        Hover = 1 << 3,

        /// <summary>
        /// Hover Event: plotly_unhover
        /// </summary>
        [EnumMember(Value = @"plotly_unhover")]
        UnHover = 1 << 4,

        /// <summary>
        /// Select Event: plotly_selecting
        /// </summary>
        [EnumMember(Value = @"plotly_selecting")]
        Selecting = 1 << 5,

        /// <summary>
        /// Select Event: plotly_selected
        /// </summary>
        [EnumMember(Value = @"plotly_selected")]
        Selected = 1 << 6,

        /// <summary>
        /// Update Data: plotly_restyle
        /// </summary>
        [EnumMember(Value = @"plotly_restyle")]
        Restyle = 1 << 7,

        /// <summary>
        /// Update Data: plotly_relayout
        /// </summary>
        [EnumMember(Value = @"plotly_relayout")]
        Relayout = 1 << 8,

        /// <summary>
        /// No Data: plotly_webglcontextlost
        /// </summary>
        [EnumMember(Value = @"plotly_webglcontextlost")]
        WebglContextLost = 1 << 9,

        /// <summary>
        /// No Data: plotly_autosize
        /// </summary>
        [EnumMember(Value = @"plotly_autosize")]
        AutoSize = 1 << 10,

        /// <summary>
        /// No Data: plotly_deselect
        /// </summary>
        [EnumMember(Value = @"plotly_deselect")]
        Deselect = 1 << 11,

        /// <summary>
        /// No Data: plotly_doubleclick
        /// </summary>
        [EnumMember(Value = @"plotly_doubleclick")]
        DoubleClick = 1 << 12,

        /// <summary>
        /// No Data: plotly_redraw
        /// </summary>
        [EnumMember(Value = @"plotly_redraw")]
        Redraw = 1 << 13,
    }
}