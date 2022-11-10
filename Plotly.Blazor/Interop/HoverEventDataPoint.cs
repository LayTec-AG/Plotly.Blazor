namespace Plotly.Blazor.Interop
{
    /// <summary>
    ///     A single hover event can have multiple selected points.
    ///     This class is used to parse the event data from the jsinterop action.
    /// </summary>
    public class HoverEventDataPoint
    {
        /// <summary>
        ///     The zero based index of the trace.
        /// </summary>
        public int TraceIndex { get; set; }

        /// <summary>
        ///     The zero based index of the point.
        /// </summary>
        public int PointIndex { get; set; }

        /// <summary>
        ///     The X-Value as an object to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </summary>
        public object X { get; set; }

        /// <summary>
        ///     The Y-Value as an object to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </summary>
        public object Y { get; set; }
    }
}
