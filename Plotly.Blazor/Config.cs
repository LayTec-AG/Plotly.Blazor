using System.Text.Json.Serialization;

namespace Plotly.Blazor
{
    /// <summary>
    /// Class Config.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="PlotlyChart"/> is responsive.
        /// </summary>
        /// <value><c>null</c> if [responsive] contains no value, <c>true</c> if [responsive]; otherwise, <c>false</c>.</value>
        [JsonPropertyName("responsive")]
        public bool? Responsive { get; set; }
    }
}
