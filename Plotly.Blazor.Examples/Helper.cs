using System;
using System.Collections.Generic;
using Plotly.Blazor.Traces.Scatter;

namespace Plotly.Blazor.Examples
{
    public static class Helper
    {
        /// <summary>
        /// Generates the data.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="method">The method.</param>
        /// <returns>Scatter.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static Scatter GenerateData(this Scatter reference, int startIndex, int stopIndex, GenerateMethod method = GenerateMethod.Sin)
        {
            (reference.X, reference.Y) = GenerateData(startIndex, stopIndex);
            return reference;
        }

        /// <summary>
        /// Generates the data.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="method">The method.</param>
        /// <returns>System.ValueTuple&lt;List&lt;System.Nullable&lt;System.Double&gt;&gt;, List&lt;System.Nullable&lt;System.Double&gt;&gt;&gt;.</returns>
        public static (List<double?> X, List<double?> Y) GenerateData(int startIndex, int stopIndex, GenerateMethod method = GenerateMethod.Sin)
        {
            var random = new Random();
            var x = new List<double?>();
            var y = new List<double?>();
            var a = 0.0;
            var b = 0.0;
            var c = 0.0;

            for (var i = startIndex; i < stopIndex; i++)
            {
                if (i % 100 == 0)
                {
                    a = 2 * random.NextDouble();
                }
                if (i % 1000 == 0)
                {
                    b = 2 * random.NextDouble();
                }
                if (i % 10000 == 0)
                {
                    c = 2 * random.NextDouble();
                }

                var spike = i % 1000 == 0 ? 10 : 0;

                x.Add(i);
                if (method == GenerateMethod.Sin)
                {
                    y.Add(2 * Math.Sin(i / 100.0) + a + b + c + spike + random.NextDouble());
                }
                else
                {
                    y.Add(2 * Math.Cos(i / 100.0) + a + b + c + spike + random.NextDouble());
                }
            }
            return (x, y);
        }
    }

    /// <summary>
    /// Enum GenerateMethod
    /// </summary>
    public enum GenerateMethod
    {
        /// <summary>
        /// Use sinus
        /// </summary>
        Sin,
        /// <summary>
        /// Use cosinus
        /// </summary>
        Cos
    }
}
