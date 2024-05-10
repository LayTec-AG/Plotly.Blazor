using System.Diagnostics.CodeAnalysis;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.Scatter3DLib.ProjectionLib;

namespace Plotly.Blazor.Examples
{
    public static class Helper
    {
        private static Random Random => new();

        /// <summary>
        ///     Adds data to an IList.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

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
        
        
        /// <summary>
        ///     Generates the data.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="stopIndex">Index of the stop.</param>
        /// <param name="method">The method.</param>
        /// <returns>
        ///     System.ValueTuple&lt;List&lt;System.Nullable&lt;System.Double&gt;&gt;, List&lt;System.Nullable&lt;
        ///     System.Double&gt;&gt;, List&lt;System.Nullable&lt;System.Double&gt;&gt;&gt;.
        /// </returns>
        public static (List<object> X, List<object> Y, List<object> Z) GenerateData3D(int startIndex, int stopIndex,
            GenerateMethod method = GenerateMethod.Sin)
        {
            var x = new List<object>();
            var y = new List<object>();
            var z = new List<object>();

            var start = Math.Min(startIndex, stopIndex);
            var stop = Math.Max(startIndex, stopIndex);

            for (var i = start; i < stop; i++)
            {
                x.Add(MathF.Sin(i));
                y.Add(MathF.Cos(i));
                z.Add(i);
            }

            return (x, y, z);
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