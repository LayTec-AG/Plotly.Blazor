using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        internal static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            IgnoreNullValues = true,
            Converters = {
                new PolymorphicConverter<ITrace>(),
                new PolymorphicConverter<ITransform>(),
                new DateTimeConverter(),
                new DateTimeOffsetConverter()
            }
        };

        /// <summary>
        ///     Draws a new plot in an div element, overwriting any existing plot.
        ///     To update an existing plot in a div, it is much more efficient to use <see cref="React" /> than to overwrite it.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>

        public static async Task NewPlot(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.newPlot",
                objectReference.Value.Id,
                objectReference.Value.Data?.Select(trace => trace?.PrepareJsInterop(SerializerOptions)),
                objectReference.Value.Layout?.PrepareJsInterop(SerializerOptions),
                objectReference.Value.Config?.PrepareJsInterop(SerializerOptions),
                objectReference.Value.Frames?.PrepareJsInterop(SerializerOptions));
        }

        /// <summary>
        ///     Can be used in its place to create a plot, but when called again on the same div will update it far more
        ///     efficiently than <see cref="NewPlot" />.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        public static async Task React(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.react",
                objectReference.Value.Id,
                objectReference.Value.Data?.Select(trace => trace?.PrepareJsInterop(SerializerOptions)),
                objectReference.Value.Layout?.PrepareJsInterop(SerializerOptions),
                objectReference.Value.Config?.PrepareJsInterop(SerializerOptions),
                objectReference.Value.Frames?.PrepareJsInterop(SerializerOptions));
        }

        /// <summary>
        ///     This function has comparable performance to <see cref="React" /> and is faster than redrawing the whole plot with <see cref="NewPlot" />.
        ///     Essentially a combination of <see cref="Restyle" /> and <see cref="Relayout" />, however you can update single traces using this function.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="data">The data to update the chart.</param>
        /// <param name="layout">The data to update the layout</param>
        /// <param name="indices">The indices of the traces to be updated.</param>
        /// <returns>Task</returns>
        public static async Task Update(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference, Dictionary<string, object> data, Dictionary<string, object> layout, IEnumerable<int> indices)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.update",
                objectReference.Value.Id,
                data,
                layout,
                indices);
        }

        /// <summary>
        ///     Can be used to add data to an existing trace.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="x">X-Values.</param>
        /// <param name="y">Y-Values</param>
        /// <param name="indizes">Indizes.</param>
        /// <param name="max">Max Points.</param>
        public static async Task ExtendTraces(this IJSRuntime jsRuntime,
            DotNetObjectReference<PlotlyChart> objectReference, IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indizes, int? max)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.extendTraces", objectReference.Value.Id, x, y, indizes, max);
        }

        /// <summary>
        ///     Can be used to prepend data to an existing trace.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="x">X-Values.</param>
        /// <param name="y">Y-Values</param>
        /// <param name="indizes">Indizes.</param>
        /// <param name="max">Max Points.</param>
        public static async Task PrependTraces(this IJSRuntime jsRuntime,
            DotNetObjectReference<PlotlyChart> objectReference, IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indizes, int? max)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.prependTraces", objectReference.Value.Id, x, y, indizes, max);
        }

        /// <summary>
        ///     Can be used to add a new trace.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="trace">The trace data.</param>
        /// <param name="index">The optional index, where to add the trace.</param>
        public static async Task AddTrace(this IJSRuntime jsRuntime,
            DotNetObjectReference<PlotlyChart> objectReference, ITrace trace, int? index)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.addTrace", objectReference.Value.Id,
                trace?.PrepareJsInterop(SerializerOptions), index);
        }

        /// <summary>
        ///     Can be used to delete a trace.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="index">The index of the trace, which should be removed.</param>
        public static async Task DeleteTrace(this IJSRuntime jsRuntime,
            DotNetObjectReference<PlotlyChart> objectReference, int index)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.deleteTrace", objectReference.Value.Id, index);
        }

        /// <summary>
        ///     Can be used to purge a chart.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        public static async Task Purge(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.purge", objectReference.Value.Id);
        }

        /// <summary>
        ///     An efficient means of updating the layout object of an existing plot.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        public static async Task Relayout(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.relayout",
                objectReference.Value.Id,
                objectReference.Value.Layout?.PrepareJsInterop(SerializerOptions));
        }

        /// <summary>
        ///     An efficient means of updating the layout object of an existing plot.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="data">The data to update the layout with.</param>
        /// <returns>Task</returns>
        public static async Task Relayout(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference, Dictionary<string, object> data)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.relayout",
                objectReference.Value.Id,
                data);
        }

        /// <summary>
        ///     Can be used to add data to an existing trace.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <param name="trace">The new trace parameter</param>
        /// <param name="indizes">Indizes.</param>
        public static async Task Restyle(this IJSRuntime jsRuntime,
            DotNetObjectReference<PlotlyChart> objectReference, ITrace trace, IEnumerable<int> indizes)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.restyle", objectReference.Value.Id, trace?.PrepareJsInterop(SerializerOptions), indizes);
        }

        /// <summary>
        ///     Subscribes the chart to listen for the plotly_relayouting event.
        /// </summary>
        /// <param name="jsRuntime">The js runtime.</param>
        /// <param name="objectReference">The object reference.</param>
        /// <returns>Task</returns>
        public static async Task SubscribeRelayoutingEvent(this IJSRuntime jsRuntime, DotNetObjectReference<PlotlyChart> objectReference)
        {
            await jsRuntime.InvokeVoidAsync($"{PlotlyInterop}.subscribeRelayoutingEvent", objectReference, objectReference.Value.Id);
        }
    }
}