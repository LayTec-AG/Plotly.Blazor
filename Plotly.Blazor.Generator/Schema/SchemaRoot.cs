using System.Collections.Generic;
using System.Text.Json.Serialization;
using Plotly.Blazor.Generator.Schema.Transforms;

namespace Plotly.Blazor.Generator.Schema
{
    /// <summary>
    ///     Class SchemaRoot.
    /// </summary>
    public class SchemaRoot
    {
        /// <summary>
        ///     Gets or sets the definitions.
        /// </summary>
        /// <value>The definitions.</value>
        [JsonPropertyName("Defs")]
        public Definitions.Definitions Definitions { get; set; }

        /// <summary>
        ///     Gets or sets the traces.
        /// </summary>
        /// <value>The traces.</value>
        public IDictionary<string, Trace.Trace> Traces { get; set; }

        /// <summary>
        ///     Gets or sets the layout.
        /// </summary>
        /// <value>The layout.</value>
        public Layout.Layout Layout { get; set; }

        /// <summary>
        ///     Gets or sets the transform list.
        /// </summary>
        /// <value>The transform list.</value>
        public IDictionary<string, Transform> Transforms { get; set; }

        /// <summary>
        ///     Gets or sets the frames.
        /// </summary>
        /// <value>The frames.</value>
        public Frames.Frames Frames { get; set; }

        /// <summary>
        ///     Gets or sets the animation.
        /// </summary>
        /// <value>The animation.</value>
        public IDictionary<string, AttributeDescription> Animation { get; set; }

        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IDictionary<string, AttributeDescription> Config { get; set; }
    }
}