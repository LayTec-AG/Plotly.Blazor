using System;

namespace Plotly.Blazor.Traces.Scatter
{
    /// <summary>
    /// Enum ScatterMode
    /// </summary>
    [Flags]
    public enum ScatterMode
    {
        /// <summary>
        /// Use none
        /// </summary>
        None = 0,
        /// <summary>
        /// Use lines
        /// </summary>
        Lines = 1,
        /// <summary>
        /// Use markers
        /// </summary>
        Markers = 2,
        /// <summary>
        /// Use text
        /// </summary>
        Text = 4
    }
}
