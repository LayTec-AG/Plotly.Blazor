using System.Collections.Generic;
using System.Text.Json;

namespace Plotly.Blazor.Generator.Schema.Layout
{
    /// <summary>
    ///     Class Layout.
    /// </summary>
    public class Layout
    {
        /// <summary>
        ///     Gets or sets the layout attributes.
        /// </summary>
        /// <value>The layout attributes.</value>
        public IDictionary<string, JsonElement> LayoutAttributes { get; set; }
    }
}