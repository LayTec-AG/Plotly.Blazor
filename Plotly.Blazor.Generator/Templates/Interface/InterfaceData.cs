using System.Collections.Generic;

namespace Plotly.Blazor.Generator.Templates.Interface
{
    /// <summary>
    ///     Class EnumeratedData.
    /// </summary>
    public class InterfaceData : Data
    {
        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public List<Property> Properties { get; set; } = new List<Property>();
    }
}