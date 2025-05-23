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

namespace Plotly.Blazor.Traces
{
    /// <summary>
    ///     The ParCoords class.
    ///     Implements the <see cref="ITrace" />.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(PlotlyConverter))]
    [Serializable]
    public class ParCoords : ITrace, IEquatable<ParCoords>
    {
        /// <inheritdoc/>
        [JsonPropertyName(@"type")]
        public TraceTypeEnum? Type { get; } = TraceTypeEnum.ParCoords;

        /// <summary>
        ///     Assigns extra data each datum. This may be useful when listening to hover,
        ///     click and selection events. Note that, <c>scatter</c> traces also appends
        ///     customdata items in the markers DOM elements
        /// </summary>
        [JsonPropertyName(@"customdata")]
        public IList<object> CustomData { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for <c>customdata</c>.
        /// </summary>
        [JsonPropertyName(@"customdatasrc")]
        public string CustomDataSrc { get; set;} 

        /// <summary>
        ///     Gets or sets the Dimensions.
        /// </summary>
        [JsonPropertyName(@"dimensions")]
        public IList<Plotly.Blazor.Traces.ParCoordsLib.Dimension> Dimensions { get; set;} 

        /// <summary>
        ///     Gets or sets the Domain.
        /// </summary>
        [JsonPropertyName(@"domain")]
        public Plotly.Blazor.Traces.ParCoordsLib.Domain Domain { get; set;} 

        /// <summary>
        ///     Assigns id labels to each datum. These ids for object constancy of data
        ///     points during animation. Should be an array of strings, not numbers or any
        ///     other type.
        /// </summary>
        [JsonPropertyName(@"ids")]
        public IList<object> Ids { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for <c>ids</c>.
        /// </summary>
        [JsonPropertyName(@"idssrc")]
        public string IdsSrc { get; set;} 

        /// <summary>
        ///     Sets the angle of the labels with respect to the horizontal. For example,
        ///     a <c>tickangle</c> of -90 draws the labels vertically. Tilted labels with
        ///     <c>labelangle</c> may be positioned better inside margins when <c>labelposition</c>
        ///     is set to <c>bottom</c>.
        /// </summary>
        [JsonPropertyName(@"labelangle")]
        public decimal? LabelAngle { get; set;} 

        /// <summary>
        ///     Sets the font for the <c>dimension</c> labels.
        /// </summary>
        [JsonPropertyName(@"labelfont")]
        public Plotly.Blazor.Traces.ParCoordsLib.LabelFont LabelFont { get; set;} 

        /// <summary>
        ///     Specifies the location of the <c>label</c>. <c>top</c> positions labels
        ///     above, next to the title <c>bottom</c> positions labels below the graph
        ///     Tilted labels with <c>labelangle</c> may be positioned better inside margins
        ///     when <c>labelposition</c> is set to <c>bottom</c>.
        /// </summary>
        [JsonPropertyName(@"labelside")]
        public Plotly.Blazor.Traces.ParCoordsLib.LabelSideEnum? LabelSide { get; set;} 

        /// <summary>
        ///     Sets the reference to a legend to show this trace in. References to these
        ///     legends are <c>legend</c>, <c>legend2</c>, <c>legend3</c>, etc. Settings
        ///     for these legends are set in the layout, under <c>layout.legend</c>, <c>layout.legend2</c>,
        ///     etc.
        /// </summary>
        [JsonPropertyName(@"legend")]
        public string Legend { get; set;} 

        /// <summary>
        ///     Gets or sets the LegendGroupTitle.
        /// </summary>
        [JsonPropertyName(@"legendgrouptitle")]
        public Plotly.Blazor.Traces.ParCoordsLib.LegendGroupTitle LegendGroupTitle { get; set;} 

        /// <summary>
        ///     Sets the legend rank for this trace. Items and groups with smaller ranks
        ///     are presented on top/left side while with <c>reversed</c> <c>legend.traceorder</c>
        ///     they are on bottom/right side. The default legendrank is 1000, so that you
        ///     can use ranks less than 1000 to place certain items before all unranked
        ///     items, and ranks greater than 1000 to go after all unranked items. When
        ///     having unranked or equal rank items shapes would be displayed after traces
        ///     i.e. according to their order in data and layout.
        /// </summary>
        [JsonPropertyName(@"legendrank")]
        public decimal? LegendRank { get; set;} 

        /// <summary>
        ///     Sets the width (in px or fraction) of the legend for this trace.
        /// </summary>
        [JsonPropertyName(@"legendwidth")]
        public decimal? LegendWidth { get; set;} 

        /// <summary>
        ///     Gets or sets the Line.
        /// </summary>
        [JsonPropertyName(@"line")]
        public Plotly.Blazor.Traces.ParCoordsLib.Line Line { get; set;} 

        /// <summary>
        ///     Assigns extra meta information associated with this trace that can be used
        ///     in various text attributes. Attributes such as trace <c>name</c>, graph,
        ///     axis and colorbar <c>title.text</c>, annotation <c>text</c> <c>rangeselector</c>,
        ///     <c>updatemenues</c> and <c>sliders</c> <c>label</c> text all support <c>meta</c>.
        ///     To access the trace <c>meta</c> values in an attribute in the same trace,
        ///     simply use <c>%{meta[i]}</c> where <c>i</c> is the index or key of the <c>meta</c>
        ///     item in question. To access trace <c>meta</c> in layout attributes, use
        ///     <c>%{data[n[.meta[i]}</c> where <c>i</c> is the index or key of the <c>meta</c>
        ///     and <c>n</c> is the trace index.
        /// </summary>
        [JsonPropertyName(@"meta")]
        public object Meta { get; set;} 

        /// <summary>
        ///     Assigns extra meta information associated with this trace that can be used
        ///     in various text attributes. Attributes such as trace <c>name</c>, graph,
        ///     axis and colorbar <c>title.text</c>, annotation <c>text</c> <c>rangeselector</c>,
        ///     <c>updatemenues</c> and <c>sliders</c> <c>label</c> text all support <c>meta</c>.
        ///     To access the trace <c>meta</c> values in an attribute in the same trace,
        ///     simply use <c>%{meta[i]}</c> where <c>i</c> is the index or key of the <c>meta</c>
        ///     item in question. To access trace <c>meta</c> in layout attributes, use
        ///     <c>%{data[n[.meta[i]}</c> where <c>i</c> is the index or key of the <c>meta</c>
        ///     and <c>n</c> is the trace index.
        /// </summary>
        [JsonPropertyName(@"meta")]
        [Array]
        public IList<object> MetaArray { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for <c>meta</c>.
        /// </summary>
        [JsonPropertyName(@"metasrc")]
        public string MetaSrc { get; set;} 

        /// <summary>
        ///     Sets the trace name. The trace name appears as the legend item and on hover.
        /// </summary>
        [JsonPropertyName(@"name")]
        public string Name { get; set;} 

        /// <summary>
        ///     Sets the font for the <c>dimension</c> range values.
        /// </summary>
        [JsonPropertyName(@"rangefont")]
        public Plotly.Blazor.Traces.ParCoordsLib.RangeFont RangeFont { get; set;} 

        /// <summary>
        ///     Gets or sets the Stream.
        /// </summary>
        [JsonPropertyName(@"stream")]
        public Plotly.Blazor.Traces.ParCoordsLib.Stream Stream { get; set;} 

        /// <summary>
        ///     Sets the font for the <c>dimension</c> tick values.
        /// </summary>
        [JsonPropertyName(@"tickfont")]
        public Plotly.Blazor.Traces.ParCoordsLib.TickFont TickFont { get; set;} 

        /// <summary>
        ///     Assign an id to this trace, Use this to provide object constancy between
        ///     traces during animations and transitions.
        /// </summary>
        [JsonPropertyName(@"uid")]
        public string UId { get; set;} 

        /// <summary>
        ///     Controls persistence of some user-driven changes to the trace: <c>constraintrange</c>
        ///     in <c>parcoords</c> traces, as well as some &#39;editable: true&#39; modifications
        ///     such as <c>name</c> and <c>colorbar.title</c>. Defaults to <c>layout.uirevision</c>.
        ///     Note that other user-driven trace attribute changes are controlled by <c>layout</c>
        ///     attributes: <c>trace.visible</c> is controlled by <c>layout.legend.uirevision</c>,
        ///     <c>selectedpoints</c> is controlled by <c>layout.selectionrevision</c>,
        ///     and <c>colorbar.(x|y)</c> (accessible with &#39;config: {editable: true}&#39;)
        ///     is controlled by <c>layout.editrevision</c>. Trace changes are tracked by
        ///     <c>uid</c>, which only falls back on trace index if no <c>uid</c> is provided.
        ///     So if your app can add/remove traces before the end of the <c>data</c> array,
        ///     such that the same trace has a different index, you can still preserve user-driven
        ///     changes if you give each trace a <c>uid</c> that stays with it as it moves.
        /// </summary>
        [JsonPropertyName(@"uirevision")]
        public object UiRevision { get; set;} 

        /// <summary>
        ///     Gets or sets the Unselected.
        /// </summary>
        [JsonPropertyName(@"unselected")]
        public Plotly.Blazor.Traces.ParCoordsLib.Unselected Unselected { get; set;} 

        /// <summary>
        ///     Determines whether or not this trace is visible. If <c>legendonly</c>, the
        ///     trace is not drawn, but can appear as a legend item (provided that the legend
        ///     itself is visible).
        /// </summary>
        [JsonPropertyName(@"visible")]
        public Plotly.Blazor.Traces.ParCoordsLib.VisibleEnum? Visible { get; set;} 

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is ParCoords other)) return false;

            return ReferenceEquals(this, obj) || Equals(other);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] ParCoords other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Type == other.Type ||
                    Type != null &&
                    Type.Equals(other.Type)
                ) && 
                (
                    Equals(CustomData, other.CustomData) ||
                    CustomData != null && other.CustomData != null &&
                    CustomData.SequenceEqual(other.CustomData)
                ) &&
                (
                    CustomDataSrc == other.CustomDataSrc ||
                    CustomDataSrc != null &&
                    CustomDataSrc.Equals(other.CustomDataSrc)
                ) && 
                (
                    Equals(Dimensions, other.Dimensions) ||
                    Dimensions != null && other.Dimensions != null &&
                    Dimensions.SequenceEqual(other.Dimensions)
                ) &&
                (
                    Domain == other.Domain ||
                    Domain != null &&
                    Domain.Equals(other.Domain)
                ) && 
                (
                    Equals(Ids, other.Ids) ||
                    Ids != null && other.Ids != null &&
                    Ids.SequenceEqual(other.Ids)
                ) &&
                (
                    IdsSrc == other.IdsSrc ||
                    IdsSrc != null &&
                    IdsSrc.Equals(other.IdsSrc)
                ) && 
                (
                    LabelAngle == other.LabelAngle ||
                    LabelAngle != null &&
                    LabelAngle.Equals(other.LabelAngle)
                ) && 
                (
                    LabelFont == other.LabelFont ||
                    LabelFont != null &&
                    LabelFont.Equals(other.LabelFont)
                ) && 
                (
                    LabelSide == other.LabelSide ||
                    LabelSide != null &&
                    LabelSide.Equals(other.LabelSide)
                ) && 
                (
                    Legend == other.Legend ||
                    Legend != null &&
                    Legend.Equals(other.Legend)
                ) && 
                (
                    LegendGroupTitle == other.LegendGroupTitle ||
                    LegendGroupTitle != null &&
                    LegendGroupTitle.Equals(other.LegendGroupTitle)
                ) && 
                (
                    LegendRank == other.LegendRank ||
                    LegendRank != null &&
                    LegendRank.Equals(other.LegendRank)
                ) && 
                (
                    LegendWidth == other.LegendWidth ||
                    LegendWidth != null &&
                    LegendWidth.Equals(other.LegendWidth)
                ) && 
                (
                    Line == other.Line ||
                    Line != null &&
                    Line.Equals(other.Line)
                ) && 
                (
                    Meta == other.Meta ||
                    Meta != null &&
                    Meta.Equals(other.Meta)
                ) && 
                (
                    Equals(MetaArray, other.MetaArray) ||
                    MetaArray != null && other.MetaArray != null &&
                    MetaArray.SequenceEqual(other.MetaArray)
                ) &&
                (
                    MetaSrc == other.MetaSrc ||
                    MetaSrc != null &&
                    MetaSrc.Equals(other.MetaSrc)
                ) && 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    RangeFont == other.RangeFont ||
                    RangeFont != null &&
                    RangeFont.Equals(other.RangeFont)
                ) && 
                (
                    Stream == other.Stream ||
                    Stream != null &&
                    Stream.Equals(other.Stream)
                ) && 
                (
                    TickFont == other.TickFont ||
                    TickFont != null &&
                    TickFont.Equals(other.TickFont)
                ) && 
                (
                    UId == other.UId ||
                    UId != null &&
                    UId.Equals(other.UId)
                ) && 
                (
                    UiRevision == other.UiRevision ||
                    UiRevision != null &&
                    UiRevision.Equals(other.UiRevision)
                ) && 
                (
                    Unselected == other.Unselected ||
                    Unselected != null &&
                    Unselected.Equals(other.Unselected)
                ) && 
                (
                    Visible == other.Visible ||
                    Visible != null &&
                    Visible.Equals(other.Visible)
                );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (Type != null) hashCode = hashCode * 59 + Type.GetHashCode();
                if (CustomData != null) hashCode = hashCode * 59 + CustomData.GetHashCode();
                if (CustomDataSrc != null) hashCode = hashCode * 59 + CustomDataSrc.GetHashCode();
                if (Dimensions != null) hashCode = hashCode * 59 + Dimensions.GetHashCode();
                if (Domain != null) hashCode = hashCode * 59 + Domain.GetHashCode();
                if (Ids != null) hashCode = hashCode * 59 + Ids.GetHashCode();
                if (IdsSrc != null) hashCode = hashCode * 59 + IdsSrc.GetHashCode();
                if (LabelAngle != null) hashCode = hashCode * 59 + LabelAngle.GetHashCode();
                if (LabelFont != null) hashCode = hashCode * 59 + LabelFont.GetHashCode();
                if (LabelSide != null) hashCode = hashCode * 59 + LabelSide.GetHashCode();
                if (Legend != null) hashCode = hashCode * 59 + Legend.GetHashCode();
                if (LegendGroupTitle != null) hashCode = hashCode * 59 + LegendGroupTitle.GetHashCode();
                if (LegendRank != null) hashCode = hashCode * 59 + LegendRank.GetHashCode();
                if (LegendWidth != null) hashCode = hashCode * 59 + LegendWidth.GetHashCode();
                if (Line != null) hashCode = hashCode * 59 + Line.GetHashCode();
                if (Meta != null) hashCode = hashCode * 59 + Meta.GetHashCode();
                if (MetaArray != null) hashCode = hashCode * 59 + MetaArray.GetHashCode();
                if (MetaSrc != null) hashCode = hashCode * 59 + MetaSrc.GetHashCode();
                if (Name != null) hashCode = hashCode * 59 + Name.GetHashCode();
                if (RangeFont != null) hashCode = hashCode * 59 + RangeFont.GetHashCode();
                if (Stream != null) hashCode = hashCode * 59 + Stream.GetHashCode();
                if (TickFont != null) hashCode = hashCode * 59 + TickFont.GetHashCode();
                if (UId != null) hashCode = hashCode * 59 + UId.GetHashCode();
                if (UiRevision != null) hashCode = hashCode * 59 + UiRevision.GetHashCode();
                if (Unselected != null) hashCode = hashCode * 59 + Unselected.GetHashCode();
                if (Visible != null) hashCode = hashCode * 59 + Visible.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Checks for equality of the left ParCoords and the right ParCoords.
        /// </summary>
        /// <param name="left">Left ParCoords.</param>
        /// <param name="right">Right ParCoords.</param>
        /// <returns>Boolean</returns>
        public static bool operator == (ParCoords left, ParCoords right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Checks for inequality of the left ParCoords and the right ParCoords.
        /// </summary>
        /// <param name="left">Left ParCoords.</param>
        /// <param name="right">Right ParCoords.</param>
        /// <returns>Boolean</returns>
        public static bool operator != (ParCoords left, ParCoords right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Gets a deep copy of this instance.
        /// </summary>
        /// <returns>ParCoords</returns>
        public ParCoords DeepClone()
        {
            return this.Copy();
        }
    }
}