namespace Plotly.Blazor.Interop
{
    /// <summary>
    ///     A single event can have multiple selected points.
    /// </summary>
    public class EventDataPoint
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
        ///     Can be an array to in 3D charts.
        /// </summary>
        public object PointNumber { get; set; }

        /// <summary>
        ///     The zero-based point number. Can be used to identify the point in combination with PointNumber or PointIndex.
        /// </summary>
        public object CurveNumber { get; set; }

        /// <summary>
        ///     The text-value as an object to be compatible to multiple data types.
        /// </summary>
        public object Text { get; set; }

        /// <summary>
        ///     The X-Value as an object to be compatible to multiple data types.
        /// </summary>
        public object X { get; set; }

        /// <summary>
        ///     The Y-Value as an object to be compatible to multiple data types.
        /// </summary>
        public object Y { get; set; }

        /// <summary>
        ///     The Z-Value as an object to be compatible to multiple data types.
        /// </summary>
        public object Z { get; set; }

        /// <summary>
        ///     The Lat as an object to be compatible to multiple data types.
        /// </summary>
        public object Lat { get; set; }

        /// <summary>
        ///     The Lon as an object to be compatible to multiple data types.
        /// </summary>
        public object Lon { get; set; }
    }
}
