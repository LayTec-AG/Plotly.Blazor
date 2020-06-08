using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Generator.Schema
{
    /// <summary>
    ///     Class PropertyDescription.
    /// </summary>
    [Serializable]
    public class AttributeDescription : ICloneable
    {
        /// <summary>
        ///     Gets or sets the type of the value.
        /// </summary>
        /// <value>The type of the value.</value>
        public string ValType { get; set; }

        /// <summary>
        ///     Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role { get; set; }

        /// <summary>
        ///     Determines the minimum of the parameters.
        /// </summary>
        /// <value>The minimum.</value>
        public object Min { get; set; }

        /// <summary>
        ///     Determines the maximum of the parameters.
        /// </summary>
        /// <value>The maximum.</value>
        public object Max { get; set; }

        /// <summary>
        ///     Gets or sets the default.
        /// </summary>
        /// <value>The default.</value>
        [JsonPropertyName("dflt")]
        public object Default { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AttributeDescription" /> is anim.
        /// </summary>
        /// <value><c>true</c> if anim; otherwise, <c>false</c>.</value>
        public bool Anim { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [coerce number].
        /// </summary>
        /// <value><c>true</c> if [coerce number]; otherwise, <c>false</c>.</value>
        public bool CoerceNumber { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [array ok].
        /// </summary>
        /// <value><c>true</c> if [array ok]; otherwise, <c>false</c>.</value>
        public bool ArrayOk { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [no blank].
        /// </summary>
        /// <value><c>true</c> if [no blank]; otherwise, <c>false</c>.</value>
        public bool NoBlank { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AttributeDescription" /> is strict.
        /// </summary>
        /// <value><c>true</c> if strict; otherwise, <c>false</c>.</value>
        public bool Strict { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [free length].
        /// </summary>
        /// <value><c>true</c> if [free length]; otherwise, <c>false</c>.</value>
        public bool FreeLength { get; set; }

        /// <summary>
        ///     Gets or sets the type of the edit.
        /// </summary>
        /// <value>The type of the edit.</value>
        public string EditType { get; set; }

        /// <summary>
        ///     Gets or sets the extras.
        /// </summary>
        /// <value>The extras.</value>
        public object[] Extras { get; set; }

        /// <summary>
        ///     Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public string[] Flags { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the dimensions.
        /// </summary>
        /// <value>The dimensions.</value>
        public object Dimensions { get; set; }

        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public JsonElement Items { get; set; } = default;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is subplot object.
        /// </summary>
        /// <value><c>true</c> if this instance is subplot object; otherwise, <c>false</c>.</value>
        [JsonPropertyName("_isSubplotObj")]
        public bool IsSubplotObj { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is linked to array.
        /// </summary>
        /// <value><c>true</c> if this instance is linked to array; otherwise, <c>false</c>.</value>
        [JsonPropertyName("_isLinkedToArray")]
        public bool IsLinkedToArray { get; set; }

        /// <summary>
        ///     Gets or sets the implied edits.
        /// </summary>
        /// <value>The implied edits.</value>
        public IDictionary<string, object> ImpliedEdits { get; set; }

        /// <summary>
        ///     Gets or sets the regex.
        /// </summary>
        /// <value>The regex.</value>
        public string Regex { get; set; }

        /// <summary>
        ///     Gets or sets the deprecated properties.
        /// </summary>
        /// <value>The deprecated properties.</value>
        [JsonPropertyName("_deprecated")]
        public IDictionary<string, AttributeDescription> DeprecatedAttributes { get; set; }

        /// <summary>
        ///     Gets or sets the other properties.
        /// </summary>
        /// <value>The other properties.</value>
        [JsonExtensionData]
        public IDictionary<string, JsonElement> OtherAttributes { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance is array.
        /// </summary>
        /// <value><c>true</c> if this instance is array; otherwise, <c>false</c>.</value>
        public bool IsArray => Role == "object" && Items.ValueKind != JsonValueKind.Undefined;

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}