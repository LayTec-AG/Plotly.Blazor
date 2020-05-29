using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Transforms
{
    /// <summary>
    ///     Class Transforms.
    /// </summary>
    public class Transform
    {
        /// <summary>
        ///     Gets or sets the edit typ.
        /// </summary>
        /// <value>The edit typ.</value>
        public string EditTyp { get; set; }

        /// <summary>
        ///     Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, JsonElement> Attributes { get; set; }

        /// <summary>
        ///     Gets or sets the other attributes.
        /// </summary>
        /// <value>The other attributes.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}