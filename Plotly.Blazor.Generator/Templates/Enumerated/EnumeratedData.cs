using System.Collections;
using System.Collections.Generic;

namespace Plotly.Blazor.Generator.Templates.Enumerated
{
    /// <summary>
    /// Class EnumeratedData.
    /// </summary>

    public class EnumeratedData
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public IEnumerable<string> Description { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public Value DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<Value> Values { get; set; }
        /// <summary>
        /// Gets a value indicating whether [should serialize default value].
        /// </summary>
        /// <value><c>true</c> if [should serialize default value]; otherwise, <c>false</c>.</value>
        public bool ShouldSerializeDefaultValue => !string.IsNullOrWhiteSpace(DefaultValue?.EnumName);
    }

    /// <summary>
    /// Class Value.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the name of the enum.
        /// </summary>
        /// <value>The name of the enum.</value>
        public string EnumName { get; set; }
    }



}
