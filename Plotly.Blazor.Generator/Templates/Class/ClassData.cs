using System.Collections.Generic;
using System.Linq;

namespace Plotly.Blazor.Generator.Templates.Class
{
    /// <summary>
    ///     Class EnumeratedData.
    /// </summary>
    public class ClassData : Data
    {
        private IEnumerable<string> description;

        private IList<Property> properties = new List<Property>();

        /// <summary>
        ///     Gets or sets the interface.
        /// </summary>
        /// <value>The interface.</value>
        public string Interface { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has a subplot property.
        /// </summary>
        /// <value><c>true</c> if this instance has a subplot property; otherwise, <c>false</c>.</value>
        public bool HasSpecialProperties => Properties.Any(p => p.IsSubplot || p.IsArrayOk);

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has a property of type IList.
        /// </summary>
        /// <value><c>true</c> if this instance has a property of type IList; otherwise, <c>false</c>.</value>
        public bool HasList => Properties.Any(p => p.TypeName.StartsWith("IList"));

        /// <inheritdoc cref="Data.Description" />
        public new IEnumerable<string> Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Interface))
                {
                    return description;
                }

                var list = description.ToList();
                list.Add($@"Implements the <see cref=""{Interface}"" />.");
                return list;
            }
            set => description = value;
        }

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IList<Property> Properties
        {
            get
            {
                if (properties == null) return null;
                for (var i = 0; i < properties.Count; i++)
                {
                    properties[i].HasMore = i != properties.Count - 1;
                }

                return properties;
            }
            set => properties = value;
        }
    }
}