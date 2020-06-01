/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;

namespace Plotly.Blazor.Traces.ChoroplethMapBoxLib.MarkerLib
{
    /// <summary>
    ///     The Line class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    public class Line 
    {
        /// <summary>
        ///     Sets themarker.linecolor. It accepts either a specific color or an array
        ///     of numbers that are mapped to the colorscale relative to the max and min
        ///     values of the array or relative to <c>marker.line.cmin</c> and <c>marker.line.cmax</c>
        ///     if set.
        /// </summary>
        [JsonPropertyName(@"color")]
        public object Color { get; set;} 

        /// <summary>
        ///     Sets the width (in px) of the lines bounding the marker points.
        /// </summary>
        [JsonPropertyName(@"width")]
        public float? Width { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for  color .
        /// </summary>
        [JsonPropertyName(@"colorsrc")]
        public string ColorSrc { get; set;} 

        /// <summary>
        ///     Sets the source reference on Chart Studio Cloud for  width .
        /// </summary>
        [JsonPropertyName(@"widthsrc")]
        public string WidthSrc { get; set;} 

    }
}