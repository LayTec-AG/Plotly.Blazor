using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Definitions
{
    /// <summary>
    ///     Class Definitions.
    /// </summary>
    public class Definitions
    {
        /// <summary>
        ///     Gets or sets the value objects.
        /// </summary>
        /// <value>The value objects.</value>
        public IDictionary<string, ValObject> ValObjects { get; set; }

        /// <summary>
        ///     Gets or sets the meta keys.
        /// </summary>
        /// <value>The meta keys.</value>
        public string[] MetaKeys { get; set; }

        /// <summary>
        ///     Gets or sets the type of the edit.
        /// </summary>
        /// <value>The type of the edit.</value>
        public IDictionary<string, AttributeDescription> EditType { get; set; }

        /// <summary>
        ///     Gets or sets the other properties.
        /// </summary>
        /// <value>The other properties.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}