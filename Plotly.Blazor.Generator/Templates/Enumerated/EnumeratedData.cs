using System.Collections.Generic;

namespace Plotly.Blazor.Generator.Templates.Enumerated
{
    /// <summary>
    ///     Class EnumeratedData.
    /// </summary>
    public class EnumeratedData : Data
    {
        /// <summary>
        ///     Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public EnumeratedValue DefaultValue { get; set; }

        /// <summary>
        ///     Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<EnumeratedValue> Values { get; set; }

        /// <summary>
        ///     Gets a value indicating whether [should serialize default value].
        /// </summary>
        /// <value><c>true</c> if [should serialize default value]; otherwise, <c>false</c>.</value>
        public bool ShouldSerializeDefaultValue => !string.IsNullOrWhiteSpace(DefaultValue?.EnumName);

        /// <summary>
        ///     Get the composed values.
        /// </summary>
        /// <returns>System.String.</returns>
        public List<string> Composed()
        {
            var sb = new List<string>();
            if (DefaultValue != null && !string.IsNullOrWhiteSpace(DefaultValue.EnumName))
            {
                sb.Add($"[EnumMember(Value=@\"{DefaultValue.DisplayName}\")]");
                sb.Add($"{DefaultValue.EnumName} = 0,");
            }

            foreach (var value in Values)
            {
                sb.Add($"[EnumMember(Value=@\"{value.DisplayName}\")]");
                sb.Add($"{value.EnumName},");
            }

            // Remove last ','
            sb[^1] = sb[^1].Remove(sb[^1].Length - 1);

            return sb;
        }
    }

    /// <summary>
    ///     Enumerated Value.
    /// </summary>
    public class EnumeratedValue
    {
        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the enum.
        /// </summary>
        /// <value>The name of the enum.</value>
        public string EnumName { get; set; }
    }
}