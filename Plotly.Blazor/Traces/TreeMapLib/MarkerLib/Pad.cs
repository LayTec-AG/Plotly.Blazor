/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.Traces.TreeMapLib.MarkerLib
{
    /// <summary>
    ///     The Pad class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Serializable]
    public class Pad : IEquatable<Pad>
    {
        /// <summary>
        ///     Sets the padding form the bottom (in px).
        /// </summary>
        [JsonPropertyName(@"b")]
        public decimal? B { get; set;} 

        /// <summary>
        ///     Sets the padding form the left (in px).
        /// </summary>
        [JsonPropertyName(@"l")]
        public decimal? L { get; set;} 

        /// <summary>
        ///     Sets the padding form the right (in px).
        /// </summary>
        [JsonPropertyName(@"r")]
        public decimal? R { get; set;} 

        /// <summary>
        ///     Sets the padding form the top (in px).
        /// </summary>
        [JsonPropertyName(@"t")]
        public decimal? T { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Pad other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Pad other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    B == other.B ||
                    B != null &&
                    B.Equals(other.B)
                ) && 
                (
                    L == other.L ||
                    L != null &&
                    L.Equals(other.L)
                ) && 
                (
                    R == other.R ||
                    R != null &&
                    R.Equals(other.R)
                ) && 
                (
                    T == other.T ||
                    T != null &&
                    T.Equals(other.T)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (B != null) hashCode = hashCode * 59 + B.GetHashCode();
                if (L != null) hashCode = hashCode * 59 + L.GetHashCode();
                if (R != null) hashCode = hashCode * 59 + R.GetHashCode();
                if (T != null) hashCode = hashCode * 59 + T.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left Pad and the right Pad.
        /// </summary>
        /// <param name="left">Left Pad.</param>
        /// <param name="right">Right Pad.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (Pad left, Pad right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left Pad and the right Pad.
        /// </summary>
        /// <param name="left">Left Pad.</param>
        /// <param name="right">Right Pad.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (Pad left, Pad right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>Pad</returns>
        public Pad DeepClone()
        {
            return this.Copy();
        }
    }
}