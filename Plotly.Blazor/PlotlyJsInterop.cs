using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Plotly.Blazor
{
    /// <summary>
    ///     Allows JsInterop functionality for Plotly.js
    /// </summary>
    public static class PlotlyJsInterop
    {
        // Use custom jsInterop functions to prevent exceptions caused by circle references in the return value
        // Should be fixed in future blazor wasm releases
        private const string PlotlyInterop = "plotlyInterop";

        /// <summary>
        /// Draws a new plot in an div element, overwriting any existing plot.
        /// To update an existing plot in a div, it is much more efficient to use <see cref="React" /> than to overwrite it.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>

        public static async Task NewPlot(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.newPlot",
                objectReference.Value.Id,
                objectReference.Value
                    .Data, // Cast is necessary! Otherwise, it would only serialize the properties of the interface
                objectReference.Value.Layout,
                objectReference.Value.Config);
        }

        /// <summary>
        /// Can be used in its place to create a plot, but when called again on the same div will update it far more
        /// efficiently than <see cref="NewPlot" />.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        public static async Task React(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.react",
                objectReference.Value.Id,
                objectReference.Value
                    .Data, // Cast is necessary! Otherwise, it would only serialize the properties of the interface
                objectReference.Value.Layout,
                objectReference.Value.Config);
        }
    }
}