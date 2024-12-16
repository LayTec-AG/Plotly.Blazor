using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Plotly.Blazor.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
        ///     Set to true, when the basic minified version of plotly.js should be used.
        /// </summary>
        [Parameter]
        public bool UseBasicVersion { get; set; }

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

        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        private PlotlyJsInterop Interop { get; set; }

        /// <inheritdoc/>
        protected override bool ShouldRender() => false;

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            Interop = new PlotlyJsInterop(JsRuntime, this, UseBasicVersion);
            base.OnInitialized();
        }

        /// <inheritdoc/>
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

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
            await Interop.React(cancellationToken);
        }

        /// <summary>
        ///     Updates the chart layout using the current properties.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Relayout(CancellationToken cancellationToken = default)
        {
            await Interop.Relayout(cancellationToken);
        }

        /// <summary>
        ///     Updates the chart layout partially using the given <paramref name="value"/> properties.
        ///     <paramref name="value"/> can be an anonymous type.
        /// </summary>
        /// <param name="value">Partial update values</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Relayout<T>(T value, CancellationToken cancellationToken = default)
        {
            if (value.Equals(default)) return;

            Layout ??= new Layout();

            var json = JsonSerializer.Serialize(value.PrepareJsInterop(PlotlyJsInterop.SerializerOptions));
            Layout.Populate(json, PlotlyJsInterop.SerializerOptions); // apply changes to the existing object

            await Interop.Relayout(value, cancellationToken);
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
            return await Interop.ToImage(format, height, width, cancellationToken);
        }

        /// <summary>
        /// Exports the given chart as a binary image string.
        /// <code>
        ///  
        /// </code>
        /// </summary>
        /// <param name="plotlyJsInterop">A PlotlyJsInterop instance to be used for interop</param>
        /// <param name="chartDefinition">The chart definition (data, config, layout).</param>
        /// <param name="format">The image format.</param>
        /// <param name="height">The image height.</param>
        /// <param name="width">The image width.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The image as a base64 encoded string.</returns>
        public static async Task<string> ToImage(PlotlyJsInterop plotlyJsInterop, ChartDefinition chartDefinition, ImageFormat format, uint height, uint width, CancellationToken cancellationToken = default)
        {
            return await plotlyJsInterop.ToImage(chartDefinition, format, height, width, cancellationToken);
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
            await Interop.DownloadImage(format, height, width, fileName, cancellationToken);
        }

        /// <summary>
        ///     Updates the specified traces using the provided parameter
        /// </summary>
        /// <param name = "trace">New trace parameter, create new object and assign only update value</param>
        /// <param name = "indices">indices of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task Restyle(ITrace trace, IEnumerable<int> indices, CancellationToken cancellationToken = default)
        {
            if (indices?.Any() != true)
            {
                throw new ArgumentException("You must specifiy at least one index.");
            }

            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            var absoluteindices = new List<int>();
            foreach (var index in indices)
            {
                var absoluteIndex = index < 0 ? Data.Count + index : index;
                if (index > Data.Count - 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                absoluteindices.Add(absoluteIndex);
            }

            var json = JsonSerializer.Serialize(trace.PrepareJsInterop(PlotlyJsInterop.SerializerOptions));
            foreach (var currentTrace in absoluteindices.Select(index => Data[index]))
            {
                currentTrace.Populate(json, PlotlyJsInterop.SerializerOptions); // apply changes to the existing object
            }

            await Interop.Restyle(trace, absoluteindices.ToArray(), cancellationToken);
        }

        /// <summary>
        ///     This function has comparable performance to <see cref="React"/> and is faster than redrawing the whole plot with <see cref="NewPlot"/> .
        ///     An efficient means of updating both the data array and layout object in an existing plot, basically a combination of <see cref="Restyle"/> and <see cref="Relayout"/>.
        /// </summary>
        /// <param name="dataUpdate">The data update, can be an anonymous type or a new trace object.</param>
        /// <param name="layoutUpdate">The layout update, can be an anonymous type or a new layout object.</param>
        /// <param name="indices">The traces to update</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public async Task Update(object dataUpdate = default, object layoutUpdate = default, IEnumerable<int> indices = default, CancellationToken cancellationToken = default)
        {
            await Interop.Update(dataUpdate, layoutUpdate, indices, cancellationToken);
        }

        /// <summary>
        ///     (Re-)Creates the chart using the current parameters.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task NewPlot(CancellationToken cancellationToken = default)
        {
            await Interop.NewPlot(cancellationToken);
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
            await Interop.AddTrace(trace, index, cancellationToken);
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
            await Interop.DeleteTrace(index, cancellationToken);
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
        ///     Extends a trace, determined by the index, with the specified object x, y, z.
        /// </summary>
        /// <param name = "x">X-Value.</param>
        /// <param name = "y">Y-Value.</param>
        /// <param name="z">Z-Value.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ExtendTrace3D(object x, object y, object z, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null && z == null)
            {
                return;
            }

            IEnumerable<object> xList = x == null ? null : new[] { x };
            IEnumerable<object> yList = y == null ? null : new[] { y };
            IEnumerable<object> zList = z == null ? null : new[] { z };

            await ExtendTrace3D(xList, yList, zList, index, max, cancellationToken);

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
        ///     Extends a trace, determined by the index  with the specified arrays x, y, z.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name="z">Y-Values.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task ExtendTrace3D(IEnumerable<object> x, IEnumerable<object> y, IEnumerable<object> z, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null && z == null)
            {
                return;
            }

            IEnumerable<IEnumerable<object>> xList = x == null ? null : new[] { x };
            IEnumerable<IEnumerable<object>> yList = y == null ? null : new[] { y };
            IEnumerable<IEnumerable<object>> zList = z == null ? null : new[] { z };

            await ExtendTraces3D(xList, yList, zList, new[] { index }, max, cancellationToken);
        }


        /// <summary>
        ///     Extends multiple traces, determined by the indices, with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "indices">Indices of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <exception cref="ArgumentException">Arguments are invalid</exception>
        /// <exception cref="InvalidOperationException">Data lists are not initialized</exception>
        /// <returns>Task</returns>
        public async Task ExtendTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indices, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indices == null)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var indicesArr = indices as int[] ?? indices.ToArray();
            if (indicesArr.Length < 1)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var xArr = x?.ToArray();
            var yArr = y?.ToArray();

            if (xArr != null && xArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indices.");
            }

            if (yArr != null && yArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indices.");
            }

            for (var i = 0; i < indicesArr.Length; i++)
            {
                var index = indicesArr[i];
                var currentTrace = index < 0 ? Data[Data.Count + index] : Data[index];
                var traceType = currentTrace.GetType();

                if (xArr != null)
                {
                    var currentXData = xArr[i];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "X", xData, max, false);
                }

                if (yArr != null)
                {
                    var currentYData = yArr[i];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Y", yData, max, false);
                }
            }

            await DataChanged.InvokeAsync(Data);
            await Interop.ExtendTraces(xArr, yArr, indicesArr, max, cancellationToken);
        }

        /// <summary>
        ///     Extends multiple traces, determined by the indices, with the specified arrays x, y, z.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "z">Z-Values.</param>
        /// <param name = "indices">Indices of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <exception cref="ArgumentException">Arguments are invalid</exception>
        /// <exception cref="InvalidOperationException">Data lists are not initialized</exception>
        /// <returns>Task</returns>
        public async Task ExtendTraces3D(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<IEnumerable<object>> z,
            IEnumerable<int> indices, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indices == null)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var indicesArr = indices as int[] ?? indices.ToArray();
            if (indicesArr.Length < 1)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var xArr = x?.ToArray();
            var yArr = y?.ToArray();
            var zArr = z?.ToArray();

            if (xArr != null && xArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indices.");
            }

            if (yArr != null && yArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indices.");
            }

            if (zArr != null && zArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Z must have as many elements as indices.");
            }

            for (var i = 0; i < indicesArr.Length; i++)
            {
                var index = indicesArr[i];
                var currentTrace = index < 0 ? Data[Data.Count + index] : Data[index];
                var traceType = currentTrace.GetType();

                if (xArr != null)
                {
                    var currentXData = xArr[i];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "X", xData, max, false);
                }

                if (yArr != null)
                {
                    var currentYData = yArr[i];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Y", yData, max, false);
                }

                if (zArr != null)
                {
                    var currentYData = zArr[i];
                    var zData = currentYData as object[] ?? currentYData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Z", zData, max, false);
                }
            }

            await DataChanged.InvokeAsync(Data);
            await Interop.ExtendTraces3D(xArr, yArr, zArr, indicesArr, max, cancellationToken);
        }


        private static void AddDataToProperty(object currentTrace, Type traceType, string propertyName, IReadOnlyCollection<object> data, int? max, bool prepend)
        {
            if (data.Count <= 0)
            {
                return;
            }

            var list = (IList<object>)traceType.GetProperty(propertyName)?.GetValue(currentTrace)
                       ?? throw new InvalidOperationException($"You must first initialise the {propertyName} list before it can be expanded, e.g. by using Plotly.Update");

            if (prepend)
            {
                list.InsertRange(0, data);
                if (max != null)
                {
                    traceType.GetProperty(propertyName)?.SetValue(currentTrace, list.Take(max.Value).ToList());
                }
            }
            else
            {
                list.AddRange(data);
                if (max != null)
                {
                    traceType.GetProperty(propertyName)?.SetValue(currentTrace, list.TakeLast(max.Value).ToList());
                }
            }
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
        ///     Prepends a trace, determined by the index, with the specified object x, y, z.
        /// </summary>
        /// <param name = "x">X-Value.</param>
        /// <param name = "y">Y-Value.</param>
        /// /// <param name = "z">Y-Value.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task PrependTrace3D(object x, object y, object z, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null && z == null)
            {
                return;
            }

            IEnumerable<object> xList = x == null ? null : new[] { x };
            IEnumerable<object> yList = y == null ? null : new[] { y };
            IEnumerable<object> zList = z == null ? null : new[] { z };

            await PrependTrace3D(xList, yList, zList, index, max, cancellationToken);

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
        ///     Prepends a trace, determined by the index, with the specified arrays x, y, z.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "z">Z-Values.</param>
        /// <param name = "index">Index of the trace. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task PrependTrace3D(IEnumerable<object> x, IEnumerable<object> y, IEnumerable<object> z, int index, int? max = null, CancellationToken cancellationToken = default)
        {
            if (x == null && y == null && z == null)
            {
                return;
            }

            IEnumerable<IEnumerable<object>> xList = x == null ? null : new[] { x };
            IEnumerable<IEnumerable<object>> yList = y == null ? null : new[] { y };
            IEnumerable<IEnumerable<object>> zList = z == null ? null : new[] { z };

            await PrependTraces3D(xList, yList, zList, new[] { index }, max, cancellationToken);
        }

        /// <summary>
        ///     Prepends multiple traces, determined by the indices, with the specified arrays x, y.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "indices">indices of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <exception cref="ArgumentException">Arguments are invalid</exception>
        /// <exception cref="InvalidOperationException">Data lists are not initialized</exception>
        /// <returns>Task</returns>
        public async Task PrependTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indices, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indices == null)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var indicesArr = indices as int[] ?? indices.ToArray();
            if (indicesArr.Length < 1)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var xArr = x?.ToArray();
            var yArr = y?.ToArray();

            if (xArr != null && xArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indices.");
            }

            if (yArr != null && yArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indices.");
            }

            for (var i = 0; i < indicesArr.Length; i++)
            {
                var index = indicesArr[i];
                var currentTrace = index < 0 ? Data[Data.Count + index] : Data[index];
                var traceType = currentTrace.GetType();

                if (xArr != null)
                {
                    var currentXData = xArr[i];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "X", xData, max, true);
                }

                if (yArr != null)
                {
                    var currentYData = yArr[i];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Y", yData, max, true);
                }
            }

            await DataChanged.InvokeAsync(Data);
            await Interop.PrependTraces(xArr, yArr, indicesArr, max, cancellationToken);
        }


        /// <summary>
        ///     Prepends multiple traces, determined by the indices, with the specified arrays x, y, z.
        /// </summary>
        /// <param name = "x">X-Values.</param>
        /// <param name = "y">Y-Values.</param>
        /// <param name = "z">Z-Values.</param>
        /// <param name = "indices">indices of the traces. Use -1 e.g. to get the last one.</param>
        /// <param name = "max">Limits the number of points in the trace.</param>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <exception cref="ArgumentException">Arguments are invalid</exception>
        /// <exception cref="InvalidOperationException">Data lists are not initialized</exception>
        /// <returns>Task</returns>
        public async Task PrependTraces3D(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<IEnumerable<object>> z, IEnumerable<int> indices, int? max = null, CancellationToken cancellationToken = default)
        {
            if (indices == null)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var indicesArr = indices as int[] ?? indices.ToArray();
            if (indicesArr.Length < 1)
            {
                throw new ArgumentException("You must specify at least one index.");
            }

            var xArr = x?.ToArray();
            var yArr = y?.ToArray();
            var zArr = z?.ToArray();

            if (xArr != null && xArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("X must have as many elements as indices.");
            }

            if (yArr != null && yArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indices.");
            }

            if (zArr != null && zArr.Length != indicesArr.Length)
            {
                throw new ArgumentException("Y must have as many elements as indices.");
            }

            for (var i = 0; i < indicesArr.Length; i++)
            {
                var index = indicesArr[i];
                var currentTrace = index < 0 ? Data[Data.Count + index] : Data[index];
                var traceType = currentTrace.GetType();

                if (xArr != null)
                {
                    var currentXData = xArr[i];
                    var xData = currentXData as object[] ?? currentXData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "X", xData, max, true);
                }

                if (yArr != null)
                {
                    var currentYData = yArr[i];
                    var yData = currentYData as object[] ?? currentYData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Y", yData, max, true);
                }

                if (zArr != null)
                {
                    var currentZData = zArr[i];
                    var zData = currentZData as object[] ?? currentZData.ToArray();
                    AddDataToProperty(currentTrace, traceType, "Z", zData, max, true);
                }
            }

            await DataChanged.InvokeAsync(Data);
            await Interop.PrependTraces3D(xArr, yArr, zArr, indicesArr, max, cancellationToken);
        }

        /// <summary>
        ///     Defines the action that should happen when the LegendClickAction is triggered.
        ///     Objects are currently required for accomodating different plot value types
        /// </summary>
        [Parameter]
        public Action<LegendEventDataPoint> LegendClickAction { get; set; }

        /// <summary>
        ///     Defines the action that should happen when the ClickEvent is triggered.
        ///     Objects are currently required for accommodating different plot value types
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
        ///     Defines the action that should happen when the SelectedEvent is triggered.
        ///     Objects are currently required for accommodating different plot value types
        /// </summary>
        [Parameter]
        public Action<IEnumerable<EventDataPoint>> SelectedAction { get; set; }

        /// <summary>
        ///     Defines the action that should happen when the RelayoutEvent is triggered.
        /// </summary>
        [Parameter]
        public Action<RelayoutEventData> RelayoutAction { get; set; }

        /// <summary>
        ///     Defines the action that should happen when the RestyleEvent is triggered.
        /// </summary>
        [Parameter]
        public Action<RestyleEventData> RestyleAction { get; set; }

        /// <summary>
        ///     Method which is called by JSRuntime once a plot has been clicked, to invoke the passed in ClickAction.
        ///     Objects are currently required for accommodating different plot value types.
        /// </summary>
        //// <param name = "eventData"></param>
        [JSInvokable("LegendClickEvent")]
        public void LegendClickEvent(LegendEventDataPoint eventData)
        {
            LegendClickAction?.Invoke(eventData);
        }

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
        ///     Method which is called by JSRuntime once a you selected plot points, to invoke the passed in SelectedAction.
        ///     Objects are currently required for accommodating different plot value types.
        /// </summary>
        /// <param name = "eventData"></param>
        [JSInvokable("SelectedEvent")]
        public void SelectedEvent(IEnumerable<EventDataPoint> eventData)
        {
            SelectedAction?.Invoke(eventData);
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
        ///     Method which is called by JSRuntime when the chart's layout has changed.
        /// </summary>
        [JSInvokable("RestyleEvent")]
        public void RestyleEvent(RestyleEventData obj)
        {
            RestyleAction?.Invoke(obj);
        }

        /// <summary>
        ///     Subscribes to the legend click event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeLegendClickEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeLegendClickEvent(cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the click event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeClickEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeClickEvent(cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the hover event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeHoverEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeHoverEvent(cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the selected event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeSelectedEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeSelectedEvent(cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the relayout event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeRelayoutEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeRelayoutEvent(cancellationToken);
        }

        /// <summary>
        ///     Subscribes to the restyle event of the chart.
        /// </summary>
        /// <param name = "cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task SubscribeRestyleEvent(CancellationToken cancellationToken = default)
        {
            await Interop.SubscribeRestyleEvent(cancellationToken);
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
            await Interop.Purge(cancellationToken);
        }
    }
}