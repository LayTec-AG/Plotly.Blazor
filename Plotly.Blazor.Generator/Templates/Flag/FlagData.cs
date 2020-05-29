using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plotly.Blazor.Generator.Templates.Flag
{
    /// <summary>
    /// Class EnumeratedData.
    /// </summary>

    public class FlagData
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
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<FlagValue> Flags { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
        public IEnumerable<FlagValue> Extras { get; set; }

        /// <summary>
        /// Get the composed values.
        /// </summary>
        /// <returns>System.String.</returns>
        public List<string> Composed()
        {
            var sb = new List<string>();
            var containsSkip = Extras?.Any(v => v.DisplayName == "skip") ?? false;
            var containsNone = Extras?.Any(v => v.DisplayName == "none") ?? false;
            var containsAll = Extras?.Any(v => v.DisplayName == "all") ?? false;

            var startValue = 0;

            if (containsSkip && containsNone)
            {
                sb.Add("[EnumMember(Value=@\"skip\")]");
                sb.Add("Skip = 0,");
                sb.Add("[EnumMember(Value=@\"none\")]");
                sb.Add("None = 1,");
                startValue = 2;
            }
            else if (containsSkip)
            {
                sb.Add("[EnumMember(Value=@\"skip\")]");
                sb.Add("Skip = 0,");
                startValue = 1;
            }
            else if (containsNone)
            {
                sb.Add("[EnumMember(Value=@\"none\")]");
                sb.Add("None = 0,");
                startValue = 1;
            }

            if (Extras != null)
            {
                foreach (var value in Extras.Where(f => f.DisplayName != "all" && f.DisplayName != "none" && f.DisplayName != "skip"))
                {
                    sb.Add($"[EnumMember(Value=@\"{value.DisplayName}\")]");
                    sb.Add($"{value.EnumName} = {startValue},");
                    startValue = startValue == 0 ? startValue + 1 : startValue * 2;
                }
            }

            foreach (var value in Flags)
            {
                sb.Add($"[EnumMember(Value=@\"{value.DisplayName}\")]");
                sb.Add($"{value.EnumName} = {startValue},");
                startValue = startValue == 0 ? startValue + 1 : startValue * 2;
            }

            // Remove ',' if 'all' does not exists
            if (!containsAll)
            {
                sb[^1] = sb[^1].Remove(sb[^1].Length - 1);
            }
            else
            {
                sb.Add("[EnumMember(Value=@\"all\")]");
                sb.Add(!Flags.Any() ? $"All" : $"All = {string.Join(" | ", Flags)}");
            }
            return sb;
        }
    }

    /// <summary>
    /// Class Value.
    /// </summary>
    public class FlagValue
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

        /// <inheritdoc />
        public override string ToString()
        {
            return EnumName;
        }
    }
}
