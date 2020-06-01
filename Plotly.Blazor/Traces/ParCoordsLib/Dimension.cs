/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Plotly.Blazor.Traces.ParCoordsLib
{
    /// <summary>
    ///     The Dimension class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    public class Dimension 
    {
        /// <summary>
        ///     The shown name of the dimension.
        /// </summary>
        [JsonPropertyName(@"label")]
        public string Label { get; set;} 

        /// <summary>
        ///     Sets the values at which ticks on this axis appear.
        /// </summary>
        [JsonPropertyName(@"tickvals")]
        public IList<object> TickVals { get; set;} 

        /// <summary>
        ///     Sets the text displayed at the ticks position via <c>tickvals</c>.
        /// </summary>
        [JsonPropertyName(@"ticktext")]
        public IList<object> TickText { get; set;} 

        /// <summary>
        ///     Sets the tick label formatting rule using d3 formatting mini-languages which
        ///     are very similar to those in Python. For numbers, see: https://github.com/d3/d3-3.x-api-reference/blob/master/Formatting.md#d3_format
        ///     And for dates see: https://github.com/d3/d3-3.x-api-reference/blob/master/Time-Formatting.md#format
        ///     We add one item to d3&#39;s date formatter: <c>%{n}f</c> for fractional
        ///     seconds with n digits. For example, &#39;2016-10-13 09:15:23.456&#39; with
        ///     tickformat <c>%H~%M~%S.%2f</c> would display <c>09~15~23.46</c>
        /// </summary>
        [JsonPropertyName(@"tickformat")]
        public string TickFormat { get; set;} 

        /// <summary>
        ///     Shows the dimension when set to <c>true</c> (the default). Hides the dimension
        ///     for <c>false</c>.
        /// </summary>
        [JsonPropertyName(@"visible")]
        public bool? Visible { get; set;} 

        /// <summary>
        ///     The domain range that represents the full, shown axis extent. Defaults to
        ///     the <c>values</c> extent. Must be an array of &#39;[fromValue, toValue]&#39;
        ///     with finite numbers as elements.
        /// </summary>
        [JsonPropertyName(@"range")]
        public IList<object> Range { get; set;} 

        /// <summary>
        ///     The domain range to which the filter on the dimension is constrained. Must
        ///     be an array of &#39;[fromValue, toValue]&#39; with &#39;fromValue &lt;=
        ///     toValue&#39;, or if <c>multiselect</c> is not disabled, you may give an
        ///     array of arrays, where each inner array is &#39;[fromValue, toValue]&#39;.
        /// </summary>
        [JsonPropertyName(@"constraintrange")]
        public IList<object> ConstraintRange { get; set;} 

        /// <summary>
        ///     Do we allow multiple selection ranges or just a single range?
        /// </summary>
        [JsonPropertyName(@"multiselect")]
        public bool? MultiSelect { get; set;} 

        /// <summary>
        ///     Dimension values. <c>values[n]</c> represents the value of the <c>n</c>th
        ///     point in the dataset, therefore the <c>values</c> vector for all dimensions
        ///     must be the same (longer vectors will be truncated). Each value must be
        ///     a finite number.
        /// </summary>
        [JsonPropertyName(@"values")]
        public IList<object> Values { get; set;} 

        /// <summary>
        ///     When used in a template, named items are created in the output figure in
        ///     addition to any items the figure already has in this array. You can modify
        ///     these items in the output figure by making your own item with <c>templateitemname</c>
        ///     matching this <c>name</c> alongside your modifications (including &#39;visible:
        ///     false&#39; or &#39;enabled: false&#39; to hide it). Has no effect outside
        ///     of a template.
        /// </summary>
        [JsonPropertyName(@"name")]
        public string Name { get; set;} 

        /// <summary>
        ///     Used to refer to a named item in this array in the template. Named items
        ///     from the template will be created even without a matching item in the input
        ///     figure, but you can modify one by making an item with <c>templateitemname</c>
        ///     matching its <c>name</c>, alongside your modifications (including &#39;visible:
        ///     false&#39; or &#39;enabled: false&#39; to hide it). If there is no template
        ///     or no matching item, this item will be hidden unless you explicitly show
        ///     it with &#39;visible: true&#39;.
        /// </summary>
        [JsonPropertyName(@"templateitemname")]
        public string TemplateItemName { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for  tickvals .
        /// </summary>
        [JsonPropertyName(@"tickvalssrc")]
        public string TickValsSrc { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for  ticktext .
        /// </summary>
        [JsonPropertyName(@"ticktextsrc")]
        public string TickTextSrc { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for  values .
        /// </summary>
        [JsonPropertyName(@"valuessrc")]
        public string ValuesSrc { get; set;} 

    }
}