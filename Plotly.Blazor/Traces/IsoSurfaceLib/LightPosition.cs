/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;

namespace Plotly.Blazor.Traces.IsoSurfaceLib
{
    /// <summary>
    ///     The LightPosition class.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", "1.0.0.0")]
    public class LightPosition 
    {
        /// <summary>
        ///     Numeric vector, representing the X coordinate for each vertex.
        /// </summary>
        [JsonPropertyName(@"x")]
        public float? X { get; set;} 

        /// <summary>
        ///     Numeric vector, representing the Y coordinate for each vertex.
        /// </summary>
        [JsonPropertyName(@"y")]
        public float? Y { get; set;} 

        /// <summary>
        ///     Numeric vector, representing the Z coordinate for each vertex.
        /// </summary>
        [JsonPropertyName(@"z")]
        public float? Z { get; set;} 

    }
}