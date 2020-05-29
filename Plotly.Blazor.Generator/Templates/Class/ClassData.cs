using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plotly.Blazor.Generator.Templates.Class
{
    /// <summary>
    /// Class EnumeratedData.
    /// </summary>

    public class ClassData
    {
        private IEnumerable<string> description;

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
        /// Gets or sets the interface.
        /// </summary>
        /// <value>The interface.</value>
        public string Interface { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has nested complex attributes.
        /// </summary>
        /// <value><c>true</c> if this instance has nested complex attributes; otherwise, <c>false</c>.</value>
        public bool HasNestedComplexAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a subplot property.
        /// </summary>
        /// <value><c>true</c> if this instance has a subplot property; otherwise, <c>false</c>.</value>

        public bool HasSubplotProperty => Properties.Any(p => p.IsSubplot);

        /// <summary>
        /// Gets or sets a value indicating whether [references transform].
        /// </summary>
        /// <value><c>true</c> if [references transform]; otherwise, <c>false</c>.</value>
        public bool ReferencesTransform { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public IEnumerable<string> Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Interface)) return description;
                var list = description.ToList();
                list.Add($@"Implements the <see cref=""{Interface}"" />");
                return list;
            }
            set => description = value;
        }

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
        /// Gets or sets a value indicating whether this instance is inherited.
        /// </summary>
        /// <value><c>true</c> if this instance is inherited; otherwise, <c>false</c>.</value>
        public bool IsInherited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is subplot.
        /// </summary>
        /// <value><c>true</c> if this instance is subplot; otherwise, <c>false</c>.</value>
        public bool IsSubplot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is polymorphic.
        /// </summary>
        /// <value><c>true</c> if this instance is polymorphic; otherwise, <c>false</c>.</value>
        public bool IsPolymorphic => TypeName == "ITransform" || TypeName == "IEnumerable<ITransform>" ||
                                     TypeName == "IEnumerable<ITrace>";
    }



}
