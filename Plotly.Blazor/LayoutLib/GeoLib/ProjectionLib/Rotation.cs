/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.LayoutLib.GeoLib.ProjectionLib
{
    /// <summary>
    ///     The Rotation class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Serializable]
    public class Rotation : IEquatable<Rotation>
    {
        /// <summary>
        ///     Rotates the map along meridians (in degrees North).
        /// </summary>
        [JsonPropertyName(@"lat")]
        public decimal? Lat { get; set;} 

        /// <summary>
        ///     Rotates the map along parallels (in degrees East). Defaults to the center
        ///     of the <c>lonaxis.range</c> values.
        /// </summary>
        [JsonPropertyName(@"lon")]
        public decimal? Lon { get; set;} 

        /// <summary>
        ///     Roll the map (in degrees) For example, a roll of <c>180</c> makes the map
        ///     appear upside down.
        /// </summary>
        [JsonPropertyName(@"roll")]
        public decimal? Roll { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Rotation other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Rotation other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Lat == other.Lat ||
                    Lat != null &&
                    Lat.Equals(other.Lat)
                ) && 
                (
                    Lon == other.Lon ||
                    Lon != null &&
                    Lon.Equals(other.Lon)
                ) && 
                (
                    Roll == other.Roll ||
                    Roll != null &&
                    Roll.Equals(other.Roll)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (Lat != null) hashCode = hashCode * 59 + Lat.GetHashCode();
                if (Lon != null) hashCode = hashCode * 59 + Lon.GetHashCode();
                if (Roll != null) hashCode = hashCode * 59 + Roll.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left Rotation and the right Rotation.
        /// </summary>
        /// <param name="left">Left Rotation.</param>
        /// <param name="right">Right Rotation.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (Rotation left, Rotation right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left Rotation and the right Rotation.
        /// </summary>
        /// <param name="left">Left Rotation.</param>
        /// <param name="right">Right Rotation.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (Rotation left, Rotation right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>Rotation</returns>
        public Rotation DeepClone()
        {
            return this.Copy();
        }
    }
}