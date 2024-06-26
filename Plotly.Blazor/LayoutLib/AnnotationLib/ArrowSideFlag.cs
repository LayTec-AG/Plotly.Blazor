/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.LayoutLib.AnnotationLib
{
    /// <summary>
    ///     Sets the annotation arrow head position.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [Flags]
    [JsonConverter(typeof(EnumConverter))]
    public enum ArrowSideFlag
    {
        [EnumMember(Value=@"none")]
        None = 0,
        [EnumMember(Value=@"end")]
        End = 1,
        [EnumMember(Value=@"start")]
        Start = 2
    }
}