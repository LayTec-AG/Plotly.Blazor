using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Plotly.Blazor
{
    /// <summary>
    /// Description of the chart as expected by the Plotly library
    /// </summary>
    public sealed class ChartDefinition
    {
        /// <summary>
        /// The list of traces that make up the chart
        /// </summary>
        [JsonPropertyName("data")]
        public IList<ITrace> Data { get; set; }

        /// <summary>
        /// The layout of the chart
        /// </summary>
        [JsonPropertyName("layout")]
        public Layout Layout { get; set; }

        /// <summary>
        /// The configuration of the chart
        /// </summary>
        [JsonPropertyName("config")]
        public Config Config { get; set; }
    }
}
