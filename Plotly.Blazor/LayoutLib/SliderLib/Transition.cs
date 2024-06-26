/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.LayoutLib.SliderLib
{
    /// <summary>
    ///     The Transition class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Serializable]
    public class Transition : IEquatable<Transition>
    {
        /// <summary>
        ///     Sets the duration of the slider transition
        /// </summary>
        [JsonPropertyName(@"duration")]
        public decimal? Duration { get; set;} 

        /// <summary>
        ///     Sets the easing function of the slider transition
        /// </summary>
        [JsonPropertyName(@"easing")]
        public Plotly.Blazor.LayoutLib.SliderLib.TransitionLib.EasingEnum? Easing { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Transition other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Transition other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Duration == other.Duration ||
                    Duration != null &&
                    Duration.Equals(other.Duration)
                ) && 
                (
                    Easing == other.Easing ||
                    Easing != null &&
                    Easing.Equals(other.Easing)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (Duration != null) hashCode = hashCode * 59 + Duration.GetHashCode();
                if (Easing != null) hashCode = hashCode * 59 + Easing.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left Transition and the right Transition.
        /// </summary>
        /// <param name="left">Left Transition.</param>
        /// <param name="right">Right Transition.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (Transition left, Transition right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left Transition and the right Transition.
        /// </summary>
        /// <param name="left">Left Transition.</param>
        /// <param name="right">Right Transition.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (Transition left, Transition right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>Transition</returns>
        public Transition DeepClone()
        {
            return this.Copy();
        }
    }
}