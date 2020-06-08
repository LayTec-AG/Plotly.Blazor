using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Trace
{
    /// <summary>
    ///     Class Attributes.
    /// </summary>
    public class Attributes
    {
        /// <summary>
        ///     Gets or sets the attribute list.
        /// </summary>
        /// <value>The attribute list.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}