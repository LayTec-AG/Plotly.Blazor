using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Plotly.Blazor.Traces;

namespace Plotly.Blazor.Examples
{
    public static class Helper
    {
        private static Random Random => new Random();

        /// <summary>
        ///     Generates the data.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="method">The method.</param>
        /// <returns>Scatter.</returns>
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static Scatter GenerateData(this Scatter reference, int startIndex, int stopIndex,
            GenerateMethod method = GenerateMethod.Sin)
        {
            (reference.X, reference.Y) = GenerateData(startIndex, stopIndex);
            return reference;
        }

        /// <summary>
        ///     Generates the data.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="method">The method.</param>
        /// <returns>
        ///     System.ValueTuple&lt;List&lt;System.Nullable&lt;System.Double&gt;&gt;, List&lt;System.Nullable&lt;
        ///     System.Double&gt;&gt;&gt;.
        /// </returns>
        public static (List<object> X, List<object> Y) GenerateData(int startIndex, int stopIndex,
            GenerateMethod method = GenerateMethod.Sin)
        {
            var x = new List<object>();
            var y = new List<object>();

            var start = Math.Min(startIndex, stopIndex);
            var stop = Math.Max(startIndex, stopIndex);

            for (var i = start; i < stop; i++)
            {
                x.Add(i);
                y.Add(i.Randomize(method));
            }

            return (x, y);
        }

        private static double Randomize(this int number, GenerateMethod method = GenerateMethod.Sin)
        {
            var a = 0.0;
            var b = 0.0;
            var c = 0.0;

            if (number % 100 == 0)
            {
                a = 2 * Random.NextDouble();
            }

            if (number % 1000 == 0)
            {
                b = 2 * Random.NextDouble();
            }

            if (number % 10000 == 0)
            {
                c = 2 * Random.NextDouble();
            }

            var spike = number % 1000 == 0 ? 10 : 0;

            if (method == GenerateMethod.Sin)
            {
                return 2 * Math.Sin(number / 100.0) + a + b + c + spike + Random.NextDouble();
            }

            return 2 * Math.Cos(number / 100.0) + a + b + c + spike + Random.NextDouble();
        }
    }

    /// <summary>
    ///     Enum GenerateMethod
    /// </summary>
    public enum GenerateMethod
    {
        /// <summary>
        ///     Use sinus
        /// </summary>
        Sin,

        /// <summary>
        ///     Use cosinus
        /// </summary>
        Cos
    }
}