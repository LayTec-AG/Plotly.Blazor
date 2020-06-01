using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema.Trace
{
    /// <summary>
    ///     Class Trace.
    /// </summary>
    public class Trace
    {
        /// <summary>
        ///     Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        public Meta Meta { get; set; }

        /// <summary>
        ///     Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public object Categories { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Trace" /> is animatable.
        /// </summary>
        /// <value><c>true</c> if animatable; otherwise, <c>false</c>.</value>
        public bool Animatable { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IDictionary<string, JsonElement> Attributes { get; set; }

        /// <summary>
        ///     Gets or sets the layout properties.
        /// </summary>
        /// <value>The deprecated properties.</value>
        public IDictionary<string, AttributeDescription> LayoutAttributes { get; set; }


        /// <summary>
        ///     Gets or sets the other attributes.
        /// </summary>
        /// <value>The other attributes.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }
    }
}