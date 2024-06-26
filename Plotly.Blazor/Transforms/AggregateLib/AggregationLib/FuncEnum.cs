/*
 * THIS FILE WAS GENERATED BY PLOTLY.BLAZOR.GENERATOR
*/

using System.Text.Json.Serialization;
using System.Runtime.Serialization;
#pragma warning disable 1591

namespace Plotly.Blazor.Transforms.AggregateLib.AggregationLib
{
    /// <summary>
    ///     Sets the aggregation function. All values from the linked <c>target</c>,
    ///     corresponding to the same value in the <c>groups</c> array, are collected
    ///     and reduced by this function. <c>count</c> is simply the number of values
    ///     in the <c>groups</c> array, so does not even require the linked array to
    ///     exist. <c>first</c> (<c>last</c>) is just the first (last) linked value.
    ///     Invalid values are ignored, so for example in <c>avg</c> they do not contribute
    ///     to either the numerator or the denominator. Any data type (numeric, date,
    ///     category) may be aggregated with any function, even though in certain cases
    ///     it is unlikely to make sense, for example a sum of dates or average of categories.
    ///     <c>median</c> will return the average of the two central values if there
    ///     is an even count. <c>mode</c> will return the first value to reach the maximum
    ///     count, in case of a tie. <c>change</c> will return the difference between
    ///     the first and last linked values. <c>range</c> will return the difference
    ///     between the min and max linked values.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("Plotly.Blazor.Generator", null)]
    [JsonConverter(typeof(EnumConverter))]
    public enum FuncEnum
    {
        [EnumMember(Value=@"first")]
        First = 0,
        [EnumMember(Value=@"count")]
        Count,
        [EnumMember(Value=@"sum")]
        Sum,
        [EnumMember(Value=@"avg")]
        Avg,
        [EnumMember(Value=@"median")]
        Median,
        [EnumMember(Value=@"mode")]
        Mode,
        [EnumMember(Value=@"rms")]
        RMs,
        [EnumMember(Value=@"stddev")]
        StdDev,
        [EnumMember(Value=@"min")]
        Min,
        [EnumMember(Value=@"max")]
        Max,
        [EnumMember(Value=@"last")]
        Last,
        [EnumMember(Value=@"change")]
        Change,
        [EnumMember(Value=@"range")]
        Range
    }
}