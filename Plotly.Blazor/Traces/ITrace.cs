namespace Plotly.Blazor.Traces
{
    /// <summary>
    /// Enum TraceType
    /// </summary>
    public enum TraceType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The scatter
        /// </summary>
        Scatter = 1
    }

    /// <summary>
    /// Interface ITrace
    /// </summary>
    public interface ITrace
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public TraceType Type { get; }
    }
}
