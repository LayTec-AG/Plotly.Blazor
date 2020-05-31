using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Frames
{
    /// <summary>
    ///     Class Frames.
    /// </summary>
    public class Frames
    {
        /// <summary>
        ///     Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role { get; set; }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public Items Items { get; set; }

        /// <summary>
        ///     Gets or sets the other attributes.
        /// </summary>
        /// <value>The other attributes.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}