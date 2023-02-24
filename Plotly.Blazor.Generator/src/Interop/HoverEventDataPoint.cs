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
        ///     The zero-based point number. Can be used to identify the point in combination with CurveNumber.
        /// </summary>
        public int? PointNumber { get; set; }

        /// <summary>
        ///     The zero-based point number. Can be used to identify the point in combination with PointNumber or PointIndex.
        /// </summary>
        public int? CurveNumber { get; set; }

        /// <summary>
        ///     The text-value as an object to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </summary>
        public object Text { get; set; }

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

        /// <summary>
        ///     The Z-Value as an object to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </summary>
        public object Z { get; set; }
    }
}
