using System.Collections.Generic;
using System.Text.Json.Serialization;
using Plotly.Blazor.Common;

namespace Plotly.Blazor.Traces.Scatter
{
    /// <summary>
    /// Class Scatter.
    /// Implements the <see cref="Plotly.Blazor.Traces.ITrace" />
    /// </summary>
    /// <seealso cref="Plotly.Blazor.Traces.ITrace" />
    public class Scatter : ITrace
    {
        /// <inheritdoc />
        [JsonPropertyName("type")]
        public TraceType Type => TraceType.Scatter;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the visible.
        /// </summary>
        /// <value>The visible.</value>
        [JsonPropertyName("visible")]
        public VisibleType Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show legend].
        /// </summary>
        /// <value><c>null</c> if [show legend] contains no value, <c>true</c> if [show legend]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("showlegend")]
        public bool? ShowLegend { get; set; }

        /// <summary>
        /// Gets or sets the legend group.
        /// </summary>
        /// <value>The legend group.</value>
        [JsonPropertyName("legendgroup")]
        public string LegendGroup { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>The opacity.</value>
        [JsonPropertyName("opacity")]
        public double? Opacity { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        [JsonPropertyName("x")]
        public List<double?> X { get; set; } = new List<double?>();

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        [JsonPropertyName("y")]
        public List<double?> Y { get; set; } = new List<double?>();

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        [JsonConverter(typeof(FlagConverter))]
        [JsonPropertyName("mode")]
        public ScatterMode Mode { get; set; }
    }
}
