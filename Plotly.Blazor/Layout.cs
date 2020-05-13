using System.Text.Json.Serialization;

namespace Plotly.Blazor
{
    /// <summary>
    /// Class Layout.
    /// </summary>
    public class Layout
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonPropertyName("title")] public string Title { get; set; }
    }
}