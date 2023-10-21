using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using Plotly.Blazor.Interop;
using System.Threading;

namespace Plotly.Blazor
{
    /// <summary>
    ///     PlotlyChart
    /// </summary>
    public partial class PlotlyChart
    {
        /// <summary>
        ///     Id of the div element.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        ///     Data of the charts.
        /// </summary>
        [Parameter]
        public IList<ITrace> Data { get; set; }

        /// <summary>
        ///     Layout of the charts.
        /// </summary>
        [Parameter]
        public Layout Layout { get; set; }

        /// <summary>
        ///     Config of the charts.
        /// </summary>
        [Parameter]
        public Config Config { get; set; }

        /// <summary>
        ///     Config the frames.
        /// </summary>
        [Parameter]
        public IList<Frames> Frames { get; set; }

        /// <summary>
        ///     Callback when the ID changed.
        /// </summary>
        [Parameter]
        public EventCallback<string> IdChanged { get; set; }

        /// <summary>
        /// Callback when the data changed.
        /// </summary>
        [Parameter]
        public EventCallback<IList<ITrace>> DataChanged { get; set; }

        /// <summary>
        ///     Callback when the layout changed.
        /// </summary>
        [Parameter]
        public EventCallback<Layout> LayoutChanged { get; set; }

        /// <summary>
        ///     Callback when the config changed.
        /// </summary>
        [Parameter]
        public EventCallback<Config> ConfigChanged { get; set; }

        /// <summary>
        ///     Callback when the frames changed.
        /// </summary>
        [Parameter]
        public EventCallback<IList<Frames>> FramesChanged { get; set; }

        /// <summary>
        ///     Additional attributes for the div-Element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        ///     Action, that should be executed after the chart has been rendered.
        ///     Useful, to subscribe to events.
        /// </summary>
        [Parameter]
        public Action AfterRender { get; set; }

        /// <summary>
        ///     Can be used later to invoke methods from javaScript to .NET.
        /// </summary>
        private DotNetObjectReference<PlotlyChart> objectReference;
        /// <inheritdoc/>
        protected override bool ShouldRender() => false;
        /// <inheritdoc/>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            objectReference ??= DotNetObjectReference.Create(this);
            if (string.IsNullOrWhiteSpace(Id))
            {
                var random = new Random();
                Id = $"PlotlyChart{random.Next(int.MinValue, int.MaxValue - 1)}";
                await IdChanged.InvokeAsync(Id);
            }

            if (Data == null)
            {
                Data = new List<ITrace>();
                await DataChanged.InvokeAsync(Data);
            }

            if (Layout == null)
            {
                Layout = new Layout();
                await LayoutChanged.InvokeAsync(Layout);
            }

            if (Config == null)
            {
                Config = new Config();
                await ConfigChanged.InvokeAsync(Config);
            }

            if (Frames == null)
            {
                Frames = new List<Frames>();
                await FramesChanged.InvokeAsync(Frames);
            }
        }

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await NewPlot();
                AfterRender?.Invoke();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        ///     Updates the chart using the current properties.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task React(CancellationToken cancellationToken = default)
        {
            await JsRuntime.React(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Updates the chart layout using the current properties.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Relayout(CancellationToken cancellationToken = default)
        {
            await JsRuntime.Relayout(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Updates all the traces using the provided parameter
        /// </summary>
        /// <param name = "trace">New trace parameter, create new object and assign only update value</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Restyle(ITrace trace, CancellationToken cancellationToken = default)
        {
            await Restyle(trace, Data?.Select((_, index) => index).ToArray(), cancellationToken);
        }

        /// <summary>
        ///     Updates the specified trace using the provided parameter
        /// </summary>
        /// <param name = "trace">New trace parameter, create new object and assign only update value</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Restyle(ITrace trace, int index, CancellationToken cancellationToken = default)
        {
            await Restyle(trace, new[] { index }, cancellationToken);
        }

        /// <summary>
        ///     Exports the given chart as a binary image string.
        ///     For example, this string can be used as src attribute for an &lt;img&gt; element.
        ///     If you use Blazor Server, make sure that the size limit of SignalR messages is large enough to process your image.
        ///     You can set the bufferSize in the Startup.cs or Program.cs
        /// <code>
        ///     services
        ///        .AddServerSideBlazor()
        ///        .AddHubOptions(o =>
        ///        {
        ///            o.MaximumReceiveMessageSize = 102400000;
        ///        });
        /// </code>
        /// </summary>
        /// <param name = "format">Format of the image.</param>
        /// <param name = "height">Height of the image.</param>
        /// <param name = "width">Width of the image.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>A binary string of the exported image</returns>
        public async Task<string> ToImage(ImageFormat format, uint height, uint width, CancellationToken cancellationToken = default)
        {
            return await JsRuntime.ToImage(objectReference, format, height, width, cancellationToken);
        }

        /// <summary>
        ///     Can be used to download the chart as an image.
        /// </summary>
        /// <param name = "format">Format of the image.</param>
        /// <param name = "height">Height of the image.</param>
        /// <param name = "width">Width of the image.</param>
        /// <param name = "fileName">Name od the image file.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task DownloadImage(ImageFormat format, uint height, uint width, string fileName, CancellationToken cancellationToken = default)
        {
            await JsRuntime.DownloadImage(objectReference, format, height, width, fileName, cancellationToken);
        }

        /// <summary>
        ///     Updates the specified traces using the provided parameter
        /// </summary>
        /// <param name = "trace">New trace parameter, create new object and assign only update value</param>
        /// <param name = "indizes">Indizes of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Restyle(ITrace trace, IEnumerable<int> indizes, CancellationToken cancellationToken = default)
        {
            if (indizes?.Any() != true)
            {
                throw new ArgumentException("You must specifiy at least one index.");
            }

            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            var absoluteIndizes = new List<int>();
            foreach (var index in indizes)
            {
                var absoluteIndex = index < 0 ? Data.Count + index : index;
                if (index > Data.Count - 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                absoluteIndizes.Add(absoluteIndex);
            }

            var json = JsonSerializer.Serialize(trace.PrepareJsInterop(PlotlyJsInterop.SerializerOptions));
            foreach (var currentTrace in absoluteIndizes.Select(index => Data[index]))
            {
                currentTrace.Populate(json, PlotlyJsInterop.SerializerOptions); // apply changes to the existing object
            }

            await JsRuntime.Restyle(objectReference, trace, absoluteIndizes.ToArray(), cancellationToken);
        }

        /// <summary>
        ///     Same as <see cref = "React"/>.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Update(CancellationToken cancellationToken = default)
        {
            await React(cancellationToken);
        }

        /// <summary>
        ///     (Re-)Creates the chart using the current parameters.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task NewPlot(CancellationToken cancellationToken = default)
        {
            await JsRuntime.NewPlot(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Adds a trace to the current data.
        ///     If no index is specified, it will append it to the end.
        /// </summary>
        /// <param name = "trace">Trace to be added.</param>
        /// <param name = "index">Optional index.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task AddTrace(ITrace trace, int? index = null, CancellationToken cancellationToken = default)
        {
            Data ??= new List<ITrace>();
            if (index == null)
            {
                Data.Add(trace);
            }
            else
            {
                index = index < 0 ? Data.Count - index : index;
                if (index > Data.Count - 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                Data.Insert(index.Value, trace);
            }

            await DataChanged.InvokeAsync(Data);
            await JsRuntime.AddTrace(objectReference, trace, index, cancellationToken);
        }

        /// <summary>
        ///     Removes a trace from the current data.
        /// </summary>
        /// <param name = "trace">Trace to be removed.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task DeleteTrace(ITrace trace, CancellationToken cancellationToken = default)
        {
            if (Data == null)
            {
                throw new ArgumentNullException(nameof(Data));
            }

            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            var index = Data.IndexOf(trace);
            if (index == -1)
            {
                return;
            }

            await DeleteTrace(index, cancellationToken);
        }

        /// <summary>
        ///     Removes a trace with the specified index.
        /// </summary>
        /// <param name = "index">Optional index.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task DeleteTrace(int index, CancellationToken cancellationToken = default)
        {
            if (Data == null)
            {
                return;
            }

            index = index < 0 ? Data.Count - index : index;
            if (index > Data.Count - 1)
            {
                return;
            }

            Data.RemoveAt(index);
            await DataChanged.InvokeAsync(Data);
            await JsRuntime.DeleteTrace(objectReference, index, cancellationToken);
        }

        /// <summary>
        ///     Extends a trace, determined by the index, with the specified object x, y.
        /// </summary>
        /// <param name = "x">X-Value.</param>
        /// <param name = "y">Y-Value.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ExtendTrace(object x, object y, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null)
            {
                return;
            }

            if (x == null)
            {
                await ExtendTrace(null, new[] { y }, index, max, cancellationToken);
            }
            else if (y == null)
            {
                await ExtendTrace(new[] { x }, null, index, max, cancellationToken);
            }
            else
            {
                await ExtendTrace(new[] { x }, new[] { y }, index, max, cancellationToken);
            }
        }

        /// <summary>
        ///     Extends a trace, determined by the index  with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ExtendTrace(IEnumerable<object> x, IEnumerable<object> y, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null)
            {
                return;
            }

            if (x == null)
            {
                await ExtendTraces(null, new[] { y }, new[] { index }, max, cancellationToken);
            }
            else if (y == null)
            {
                await ExtendTraces(new[] { x }, null, new[] { index }, max, cancellationToken);
            }
            else
            {
                await ExtendTraces(new[] { x }, new[] { y }, new[] { index }, max, cancellationToken);
            }
        }

        /// <summary>
        ///     Extends multiple traces, determined by the indizes, with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "indizes">Indizes of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ExtendTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indizes, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indizes?.Any() != true)
            {
                throw new ArgumentException("You must specifiy at least one index.");
            }

            var indizesArr = indizes as int[] ?? indizes.ToArray();
            if (x != null && x.Count() != indizesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indizes.");
            }

            if (y != null && y.Count() != indizesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indizes.");
            }

            // Add the data to the current traces
            foreach (var index in indizesArr.Select((Value, Index) => new { Value, Index }))
            {
                var currentTrace = index.Value < 0 ? Data[Data.Count + index.Value] : Data[index.Value];
                var traceType = currentTrace.GetType();
                var xArray = x as IEnumerable<object>[] ?? x.ToArray();
                if (xArray.Length > 0)
                {
                    var currentXData = xArray[index.Index];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    if (xData.Length > 0)
                    {
                        var list = (IList<object>)traceType.GetProperty("X")?.GetValue(currentTrace);
                        list.AddRange(xData);
                        if (max != null)
                        {
                            traceType.GetProperty("X")?.SetValue(currentTrace, list?.TakeLast(max.Value).ToList());
                        }
                    }
                }

                var yArray = y as IEnumerable<object>[] ?? y.ToArray();
                if (yArray.Length > 0)
                {
                    var currentYData = yArray[index.Index];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    if (yData.Length > 0)
                    {
                        var list = (IList<object>)traceType.GetProperty("Y")?.GetValue(currentTrace);
                        list.AddRange(yData);
                        if (max != null)
                        {
                            traceType.GetProperty("Y")?.SetValue(currentTrace, list?.TakeLast(max.Value).ToList());
                        }
                    }
                }
            }

            await DataChanged.InvokeAsync(Data);
            await JsRuntime.ExtendTraces(objectReference, x, y, indizesArr, max, cancellationToken);
        }

        /// <summary>
        ///     Prepends a trace, determined by the index, with the specified object x, y.
        /// </summary>
        /// <param name = "x">X-Value.</param>
        /// <param name = "y">Y-Value.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task PrependTrace(object x, object y, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null)
            {
                return;
            }

            if (x == null)
            {
                await PrependTrace(null, new[] { y }, index, max, cancellationToken);
            }
            else if (y == null)
            {
                await PrependTrace(new[] { x }, null, index, max, cancellationToken);
            }
            else
            {
                await PrependTrace(new[] { x }, new[] { y }, index, max, cancellationToken);
            }
        }

        /// <summary>
        ///     Prepends a trace, determined by the index, with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task PrependTrace(IEnumerable<object> x, IEnumerable<object> y, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null)
            {
                return;
            }

            if (x == null)
            {
                await PrependTraces(null, new[] { y }, new[] { index }, max, cancellationToken);
            }
            else if (y == null)
            {
                await PrependTraces(new[] { x }, null, new[] { index }, max, cancellationToken);
            }
            else
            {
                await PrependTraces(new[] { x }, new[] { y }, new[] { index }, max, cancellationToken);
            }
        }

        /// <summary>
        ///     Prepends multiple traces, determined by the indizes, with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "indizes">Indizes of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task PrependTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indizes, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indizes?.Any() != true)
            {
                throw new ArgumentException("You must specifiy at least one index.");
            }

            var indizesArr = indizes as int[] ?? indizes.ToArray();
            if (x != null && x.Count() != indizesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indizes.");
            }

            if (y != null && y.Count() != indizesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indizes.");
            }

            // Add the data to the current traces
            foreach (var index in indizes)
            {
                var currentTrace = index < 0 ? Data[^index] : Data[index];
                var traceType = currentTrace.GetType();
                var xArray = x as IEnumerable<object>[] ?? x.ToArray();
                if (xArray.Length > 0)
                {
                    var currentXData = index < 0 ? xArray[^1] : xArray[index];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    var list = (IList<object>)traceType.GetProperty("X")?.GetValue(currentTrace);
                    if (xData.Length > 0)
                    {
                        list.InsertRange(0, xData);
                    }

                    if (max != null)
                    {
                        traceType.GetProperty("X")?.SetValue(currentTrace, list?.Take(max.Value).ToList());
                    }
                }

                var yArray = y as IEnumerable<object>[] ?? y.ToArray();
                if (yArray.Length > 0)
                {
                    var currentYData = index < 0 ? yArray[yArray.Length + index] : yArray[index];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    var list = (IList<object>)traceType.GetProperty("Y")?.GetValue(currentTrace);
                    if (yData.Length > 0)
                    {
                        list.InsertRange(0, yData);
                    }

                    if (max != null)
                    {
                        traceType.GetProperty("Y")?.SetValue(currentTrace, list?.Take(max.Value).ToList());
                    }
                }
            }

            await DataChanged.InvokeAsync(Data);
            await JsRuntime.PrependTraces(objectReference, x, y, indizes, max, cancellationToken);
        }

        /// <summary>
        ///     Defines the action that should happen when the ClickEvent is triggered.
        ///     Objects are currently required for accomodating different plot value types
        /// </summary>
        [Parameter]
        public Action<IEnumerable<EventDataPoint>> ClickAction { get; set; }

        /// <summary>
        ///     Defines the action that should happen when the HoverEvent is triggered.
        ///     Objects are currently required for accommodating different plot value types
        /// </summary>
        [Parameter]
        public Action<IEnumerable<EventDataPoint>> HoverAction { get; set; }

        /// <summary>
        ///     Defines the action that should happen when the RelayoutEvent is triggered.
        /// </summary>
        [Parameter]
        public Action<RelayoutEventData> RelayoutAction { get; set; }

        /// <summary>
        ///     Method which is called by JSRuntime once a plot has been clicked, to invoke the passed in ClickAction.
        ///     Objects are currently required for accommodating different plot value types.
        /// </summary>
        //// <param name = "eventData"></param>
        [JSInvokable("ClickEvent")]
        public void ClickEvent(IEnumerable<EventDataPoint> eventData)
        {
            ClickAction?.Invoke(eventData);
        }

        /// <summary>
        ///     Method which is called by JSRuntime once a you hover over plot points, to invoke the passed in HoverAction.
        ///     Objects are currently required for accommodating different plot value types.
        /// </summary>
        /// <param name = "eventData"></param>
        [JSInvokable("HoverEvent")]
        public void HoverEvent(IEnumerable<EventDataPoint> eventData)
        {
            HoverAction?.Invoke(eventData);
        }

        /// <summary>
        ///     Method which is called by JSRuntime when the chart's layout has changed.
        /// </summary>
        [JSInvokable("RelayoutEvent")]
        public void RelayoutEvent(RelayoutEventData obj)
        {
            RelayoutAction?.Invoke(obj);
        }

        /// <summary>
        ///     Subscribes to the click event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeClickEvent(CancellationToken cancellationToken = default)
        {
            await JsRuntime.SubscribeClickEvent(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the hover event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeHoverEvent(CancellationToken cancellationToken = default)
        {
            await JsRuntime.SubscribeHoverEvent(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the relayout event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeRelayoutEvent(CancellationToken cancellationToken = default)
        {
            await JsRuntime.SubscribeRelayoutEvent(objectReference, cancellationToken);
        }

        /// <summary>
        ///     Clears the traces from the current chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Clear(CancellationToken cancellationToken = default)
        {
            Data.Clear();
            await DataChanged.InvokeAsync(Data);
            await React(cancellationToken);
        }

        /// <summary>
        ///     Will clear the div, and remove any Plotly plots that have been placed in it.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Purge(CancellationToken cancellationToken = default)
        {
            Data.Clear();
            await DataChanged.InvokeAsync(Data);
            Layout = new Layout();
            await LayoutChanged.InvokeAsync(Layout);
            Config = new Config();
            await ConfigChanged.InvokeAsync(Config);
            Frames.Clear();
            await FramesChanged.InvokeAsync(Frames);
            await JsRuntime.Purge(objectReference, cancellationToken);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            objectReference?.Dispose();
        }
    }
}