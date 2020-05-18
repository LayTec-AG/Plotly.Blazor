using System;
// ReSharper disable UnusedParameter.Global

namespace Plotly.Blazor.Common
{
    public static class Helper
    {
        /// <summary>
        /// Gets the composition using '+' as join char.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentException">Type must be an enumerable</exception>
        /// <exception cref="ArgumentException">Type must have a flag attribute</exception>
        public static string GetComposition<T>(this T enumerator) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type must be an enumerable");
            }

            if (typeof(T).GetCustomAttributes(typeof(FlagsAttribute), false).Length == 0)
            {
                throw new ArgumentException("Type must have a flag attribute");
            }

            return enumerator.ToString().Replace(", ", "+");
        }
    }
}
