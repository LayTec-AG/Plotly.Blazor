using System.Collections.Generic;
using System.Reflection;

namespace Plotly.Blazor.Generator.Templates.Interface
{
    /// <summary>
    /// Class EnumeratedData.
    /// </summary>

    public class InterfaceData
    {
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the generator name.
        /// </summary>
        /// <value>The generator name.</value>
        public string GeneratorName => Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// Gets the generator version.
        /// </summary>
        /// <value>The generator version.</value>
        public System.Version GeneratorVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public IEnumerable<string> Description { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IEnumerable<Property> Properties { get; set; } = new List<Property>();
    }

    /// <summary>
    /// Class Value.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the property description.
        /// </summary>
        /// <value>The property description.</value>
        public IEnumerable<string> PropertyDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the enum.
        /// </summary>
        /// <value>The name of the enum.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly { get; set; }
    }



}
