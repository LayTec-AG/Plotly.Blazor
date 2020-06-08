using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Trace
{
    /// <summary>
    ///     Class Meta.
    /// </summary>
    public class Meta
    {
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the other attributes.
        /// </summary>
        /// <value>The other attributes.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}