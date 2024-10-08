/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.LayoutLib.MapLib.LayerLib
{
    /// <summary>
    ///     The Fill class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Serializable]
    public class Fill : IEquatable<Fill>
    {
        /// <summary>
        ///     Sets the fill outline color (map.layer.paint.fill-outline-color). Has an
        ///     effect only when <c>type</c> is set to <c>fill</c>.
        /// </summary>
        [JsonPropertyName(@"outlinecolor")]
        public object OutlineColor { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Fill other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Fill other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    OutlineColor == other.OutlineColor ||
                    OutlineColor != null &&
                    OutlineColor.Equals(other.OutlineColor)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (OutlineColor != null) hashCode = hashCode * 59 + OutlineColor.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left Fill and the right Fill.
        /// </summary>
        /// <param name="left">Left Fill.</param>
        /// <param name="right">Right Fill.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (Fill left, Fill right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left Fill and the right Fill.
        /// </summary>
        /// <param name="left">Left Fill.</param>
        /// <param name="right">Right Fill.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (Fill left, Fill right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>Fill</returns>
        public Fill DeepClone()
        {
            return this.Copy();
        }
    }
}