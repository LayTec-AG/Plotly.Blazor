/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor
{
    /// <summary>
    ///     Determines the type of the transform.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum TransformTypeEnum
    {
        [EnumMember(Value=@"aggregate")]
        Aggregate,
        [EnumMember(Value=@"filter")]
        Filter,
        [EnumMember(Value=@"groupby")]
        GroupBy,
        [EnumMember(Value=@"sort")]
        Sort
    }
}