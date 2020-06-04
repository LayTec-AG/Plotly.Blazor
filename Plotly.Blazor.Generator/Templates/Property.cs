using System;
using System.Collections.Generic;

namespace Plotly.Blazor.Generator.Templates
{
    /// <summary>
    ///     Class Value.
    /// </summary>
    public class Property : ICloneable
    {
        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the property description.
        /// </summary>
        /// <value>The property description.</value>
        public IEnumerable<string> PropertyDescription { get; set; }

        /// <summary>
        ///     Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enum.
        /// </summary>
        /// <value>The name of the enum.</value>
        public string PropertyName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is inherited.
        /// </summary>
        /// <value><c>true</c> if this instance is inherited; otherwise, <c>false</c>.</value>
        public bool IsInherited { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is subplot.
        /// </summary>
        /// <value><c>true</c> if this instance is subplot; otherwise, <c>false</c>.</value>
        public bool IsSubplot { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance allows arrays too.
        /// </summary>
        /// <value><c>true</c> if this instance allows arrays too.; otherwise, <c>false</c>.</value>
        public bool IsArrayOk { get; set; }

        /// <summary>
        ///     Determines if this property has IList as a type.
        /// </summary>
        public bool IsList => TypeName.StartsWith("IList");

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        ///     Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public string DefaultValue { get; set; }

        /// <summary>
        ///     Used for generation. Determines if this property is the last one of a list.
        /// </summary>
        public bool HasMore { get; set; }

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return PropertyName;
        }
    }
}