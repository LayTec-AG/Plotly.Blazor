/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;

namespace Plotly.Blazor.LayoutLib
{
    /// <summary>
    ///     The Scene class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Serializable]
    public class Scene : IEquatable<Scene>
    {
        /// <summary>
        ///     Gets or sets the Annotations.
        /// </summary>
        [JsonPropertyName(@"annotations")]
        public IList<Plotly.Blazor.LayoutLib.SceneLib.Annotation> Annotations { get; set;} 

        /// <summary>
        ///     If <c>cube</c>, this scene&#39;s axes are drawn as a cube, regardless of
        ///     the axes&#39; ranges. If <c>data</c>, this scene&#39;s axes are drawn in
        ///     proportion with the axes&#39; ranges. If <c>manual</c>, this scene&#39;s
        ///     axes are drawn in proportion with the input of <c>aspectratio</c> (the default
        ///     behavior if <c>aspectratio</c> is provided). If <c>auto</c>, this scene&#39;s
        ///     axes are drawn using the results of <c>data</c> except when one axis is
        ///     more than four times the size of the two others, where in that case the
        ///     results of <c>cube</c> are used.
        /// </summary>
        [JsonPropertyName(@"aspectmode")]
        public Plotly.Blazor.LayoutLib.SceneLib.AspectModeEnum? AspectMode { get; set;} 

        /// <summary>
        ///     Sets this scene&#39;s axis aspectratio.
        /// </summary>
        [JsonPropertyName(@"aspectratio")]
        public Plotly.Blazor.LayoutLib.SceneLib.AspectRatio AspectRatio { get; set;} 

        /// <summary>
        ///     Gets or sets the BgColor.
        /// </summary>
        [JsonPropertyName(@"bgcolor")]
        public object BgColor { get; set;} 

        /// <summary>
        ///     Gets or sets the Camera.
        /// </summary>
        [JsonPropertyName(@"camera")]
        public Plotly.Blazor.LayoutLib.SceneLib.Camera Camera { get; set;} 

        /// <summary>
        ///     Gets or sets the Domain.
        /// </summary>
        [JsonPropertyName(@"domain")]
        public Plotly.Blazor.LayoutLib.SceneLib.Domain Domain { get; set;} 

        /// <summary>
        ///     Determines the mode of drag interactions for this scene.
        /// </summary>
        [JsonPropertyName(@"dragmode")]
        public Plotly.Blazor.LayoutLib.SceneLib.DragModeEnum? DragMode { get; set;} 

        /// <summary>
        ///     Determines the mode of hover interactions for this scene.
        /// </summary>
        [JsonPropertyName(@"hovermode")]
        public Plotly.Blazor.LayoutLib.SceneLib.HoverModeEnum? HoverMode { get; set;} 

        /// <summary>
        ///     Controls persistence of user-driven changes in camera attributes. Defaults
        ///     to <c>layout.uirevision</c>.
        /// </summary>
        [JsonPropertyName(@"uirevision")]
        public object UiRevision { get; set;} 

        /// <summary>
        ///     Gets or sets the XAxis.
        /// </summary>
        [JsonPropertyName(@"xaxis")]
        public Plotly.Blazor.LayoutLib.SceneLib.XAxis XAxis { get; set;} 

        /// <summary>
        ///     Gets or sets the YAxis.
        /// </summary>
        [JsonPropertyName(@"yaxis")]
        public Plotly.Blazor.LayoutLib.SceneLib.YAxis YAxis { get; set;} 

        /// <summary>
        ///     Gets or sets the ZAxis.
        /// </summary>
        [JsonPropertyName(@"zaxis")]
        public Plotly.Blazor.LayoutLib.SceneLib.ZAxis ZAxis { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Scene other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Scene other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Equals(Annotations, other.Annotations) ||
                    Annotations != null && other.Annotations != null &&
                    Annotations.SequenceEqual(other.Annotations)
                ) &&
                (
                    AspectMode == other.AspectMode ||
                    AspectMode != null &&
                    AspectMode.Equals(other.AspectMode)
                ) && 
                (
                    AspectRatio == other.AspectRatio ||
                    AspectRatio != null &&
                    AspectRatio.Equals(other.AspectRatio)
                ) && 
                (
                    BgColor == other.BgColor ||
                    BgColor != null &&
                    BgColor.Equals(other.BgColor)
                ) && 
                (
                    Camera == other.Camera ||
                    Camera != null &&
                    Camera.Equals(other.Camera)
                ) && 
                (
                    Domain == other.Domain ||
                    Domain != null &&
                    Domain.Equals(other.Domain)
                ) && 
                (
                    DragMode == other.DragMode ||
                    DragMode != null &&
                    DragMode.Equals(other.DragMode)
                ) && 
                (
                    HoverMode == other.HoverMode ||
                    HoverMode != null &&
                    HoverMode.Equals(other.HoverMode)
                ) && 
                (
                    UiRevision == other.UiRevision ||
                    UiRevision != null &&
                    UiRevision.Equals(other.UiRevision)
                ) && 
                (
                    XAxis == other.XAxis ||
                    XAxis != null &&
                    XAxis.Equals(other.XAxis)
                ) && 
                (
                    YAxis == other.YAxis ||
                    YAxis != null &&
                    YAxis.Equals(other.YAxis)
                ) && 
                (
                    ZAxis == other.ZAxis ||
                    ZAxis != null &&
                    ZAxis.Equals(other.ZAxis)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (Annotations != null) hashCode = hashCode * 59 + Annotations.GetHashCode();
                if (AspectMode != null) hashCode = hashCode * 59 + AspectMode.GetHashCode();
                if (AspectRatio != null) hashCode = hashCode * 59 + AspectRatio.GetHashCode();
                if (BgColor != null) hashCode = hashCode * 59 + BgColor.GetHashCode();
                if (Camera != null) hashCode = hashCode * 59 + Camera.GetHashCode();
                if (Domain != null) hashCode = hashCode * 59 + Domain.GetHashCode();
                if (DragMode != null) hashCode = hashCode * 59 + DragMode.GetHashCode();
                if (HoverMode != null) hashCode = hashCode * 59 + HoverMode.GetHashCode();
                if (UiRevision != null) hashCode = hashCode * 59 + UiRevision.GetHashCode();
                if (XAxis != null) hashCode = hashCode * 59 + XAxis.GetHashCode();
                if (YAxis != null) hashCode = hashCode * 59 + YAxis.GetHashCode();
                if (ZAxis != null) hashCode = hashCode * 59 + ZAxis.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left Scene and the right Scene.
        /// </summary>
        /// <param name="left">Left Scene.</param>
        /// <param name="right">Right Scene.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (Scene left, Scene right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left Scene and the right Scene.
        /// </summary>
        /// <param name="left">Left Scene.</param>
        /// <param name="right">Right Scene.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (Scene left, Scene right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>Scene</returns>
        public Scene DeepClone()
        {
            return this.Copy();
        }
    }
}