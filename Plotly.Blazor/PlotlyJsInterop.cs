using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Plotly.Blazor;

/// <summary>
///     Allows JsInterop functionality for Plotly.js
/// </summary>
public class PlotlyJsInterop
{
    private const string InteropPath = "./_content/Plotly.Blazor/plotly-interop-5.3.0.js";
    private const string PlotlyPath = "./_content/Plotly.Blazor/plotly-2.35.2.min.js";
    private const string PlotlyBasicPath = "./_content/Plotly.Blazor/plotly-basic-1.58.5.min.js";

    private readonly DotNetObjectReference<PlotlyChart> dotNetObj;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    /// <summary>
    ///     Creates a new instance of <see cref="PlotlyJsInterop"/>.
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="chart"></param>
    /// <param name="useBasicVersion"></param>
    public PlotlyJsInterop(IJSRuntime jsRuntime, PlotlyChart chart, bool useBasicVersion)
    {
        dotNetObj = DotNetObjectReference.Create(chart);
        moduleTask = new(LoadModulesAsync(jsRuntime, useBasicVersion));
    }

    private static async Task<IJSObjectReference> LoadModulesAsync(IJSRuntime jsRuntime, bool useBasicVersion)
    {
        var jsObject = await jsRuntime.InvokeAsync<IJSObjectReference>("import", InteropPath);

        await jsObject.InvokeVoidAsync("importScript", "plotly-import", useBasicVersion ? PlotlyBasicPath : PlotlyPath);

        return jsObject;
    }

    internal static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new PolymorphicConverter<ITrace>(),
            new PolymorphicConverter<ITransform>(),
            new DateTimeConverter(),
            new DateTimeOffsetConverter()
        }
    };

    /// <summary>
    ///     Can be used to add a new trace.
    /// </summary>
    /// <param name="trace">The trace data.</param>
    /// <param name="index">The optional index, where to add the trace.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task AddTrace(ITrace trace, int? index, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;
        await jsRuntime.InvokeVoidAsync("addTrace",
            cancellationToken,
            dotNetObj.Value.Id,
            trace?.PrepareJsInterop(SerializerOptions), index);
    }

    /// <summary>
    ///     Can be used to delete a trace.
    /// </summary>
    /// <param name="index">The index of the trace, which should be removed.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task DeleteTrace(int index, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("deleteTrace",
            cancellationToken,
            dotNetObj.Value.Id, index);
    }

    /// <summary>
    ///     Can be used to download the chart as an image.
    /// </summary>
    /// <param name="format">Format of the image.</param>
    /// <param name="height">Height of the image.</param>
    /// <param name="width">Width of the image.</param>
    /// <param name="fileName">Name od the image file.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task<string> DownloadImage(ImageFormat format, uint height, uint width, string fileName, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        return await jsRuntime.InvokeAsync<string>("downloadImage", cancellationToken,
            dotNetObj.Value.Id, format, height, width, fileName);
    }

    /// <summary>
    ///     Can be used to add data to an existing trace.
    /// </summary>
    /// <param name="x">X-Values.</param>
    /// <param name="y">Y-Values</param>
    /// <param name="indices">Indices.</param>
    /// <param name="max">Max Points.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task ExtendTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indices, int? max, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("extendTraces",
            cancellationToken,
            dotNetObj.Value.Id,
            x, y, indices, max);
    }


    /// <summary>
    ///     Can be used to add data to an existing trace.
    /// </summary>
    /// <param name="x">X-Values.</param>
    /// <param name="y">Y-Values</param>
    /// <param name="z"></param>
    /// <param name="indices">Indices.</param>
    /// <param name="max">Max Points.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task ExtendTraces3D(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<IEnumerable<object>> z, IEnumerable<int> indices, int? max, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("extendTraces3D",
            cancellationToken,
            dotNetObj.Value.Id,
            x, y, z, indices, max);
    }

    /// <summary>
    ///     Draws a new plot in an div element, overwriting any existing plot.
    ///     To update an existing plot in a div, it is much more efficient to use <see cref="React" /> than to overwrite it.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task NewPlot(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("newPlot",
            cancellationToken,
            dotNetObj.Value.Id,
            dotNetObj.Value.Data?.Select(trace => trace?.PrepareJsInterop(SerializerOptions)),
            dotNetObj.Value.Layout?.PrepareJsInterop(SerializerOptions),
            dotNetObj.Value.Config?.PrepareJsInterop(SerializerOptions),
            dotNetObj.Value.Frames?.PrepareJsInterop(SerializerOptions));
    }

    /// <summary>
    ///     Can be used to prepend data to an existing trace.
    /// </summary>
    /// <param name="x">X-Values.</param>
    /// <param name="y">Y-Values</param>
    /// <param name="indices">Indices.</param>
    /// <param name="max">Max Points.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task PrependTraces(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<int> indices, int? max, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("prependTraces",
            cancellationToken,
            dotNetObj.Value.Id,
            x, y, indices, max);
    }

    /// <summary>
    ///     Can be used to prepend data to an existing 3D trace.
    /// </summary>
    /// <param name="x">X-Values.</param>
    /// <param name="y">Y-Values</param>
    /// <param name="z"></param>
    /// <param name="indices">Indices.</param>
    /// <param name="max">Max Points.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task PrependTraces3D(IEnumerable<IEnumerable<object>> x, IEnumerable<IEnumerable<object>> y, IEnumerable<IEnumerable<object>> z, IEnumerable<int> indices, int? max, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("prependTraces3D",
            cancellationToken,
            dotNetObj.Value.Id,
            x, y, z, indices, max);
    }

    /// <summary>
    ///     Can be used to purge a chart.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task Purge(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("purge", cancellationToken, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used in its place to create a plot, but when called again on the same div will update it far more
    ///     efficiently than <see cref="NewPlot" />.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task React(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("react",
            cancellationToken,
            dotNetObj.Value.Id,
            dotNetObj.Value.Data?.Select(trace => trace?.PrepareJsInterop(SerializerOptions)),
            dotNetObj.Value.Layout?.PrepareJsInterop(SerializerOptions),
            dotNetObj.Value.Config?.PrepareJsInterop(SerializerOptions),
            dotNetObj.Value.Frames?.PrepareJsInterop(SerializerOptions));
    }

    /// <summary>
    ///     An efficient means of updating the layout object of an existing plot.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task Relayout(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("relayout",
            cancellationToken,
            dotNetObj.Value.Id,
            dotNetObj.Value.Layout?.PrepareJsInterop(SerializerOptions));
    }

    /// <summary>
    ///     An efficient means of updating the layout object of an existing plot.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task Relayout<T>(T value, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("relayout",
            cancellationToken,
            dotNetObj.Value.Id,
            value?.PrepareJsInterop(SerializerOptions));
    }

    /// <summary>
    ///     Can be used to add data to an existing trace.
    /// </summary>
    /// <param name="trace">The new trace parameter</param>
    /// <param name="indices">Indices.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task Restyle(ITrace trace, IEnumerable<int> indices, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("restyle", cancellationToken,
            dotNetObj.Value.Id, trace?.PrepareJsInterop(SerializerOptions), indices);
    }

    /// <summary>
    ///     Can be used to subscribe click events for legend.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task SubscribeLegendClickEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeLegendClickEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to subscribe click events for points.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task SubscribeClickEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeClickEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to subscribe hover events for points.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task SubscribeHoverEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeHoverEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to subscribe selected events for points.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task SubscribeSelectedEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeSelectedEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to subscribe to relayout events.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task SubscribeRelayoutEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeRelayoutEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to subscribe to restyle events.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task SubscribeRestyleEvent(CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        await jsRuntime.InvokeVoidAsync("subscribeRestyleEvent", cancellationToken, dotNetObj, dotNetObj.Value.Id);
    }

    /// <summary>
    ///     Can be used to export the chart as a static image and returns a binary string of the exported image.
    /// </summary>
    /// <param name="format">Format of the image.</param>
    /// <param name="height">Height of the image.</param>
    /// <param name="width">Width of the image.</param>
    /// <returns>Binary string of the exported image.</returns>
    /// <param name="cancellationToken">CancellationToken</param>
    public async Task<string> ToImage(ImageFormat format, uint height, uint width, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        return await jsRuntime.InvokeAsync<string>("toImage", cancellationToken, dotNetObj.Value.Id, format, height, width);
    }

    /// <summary>
    ///     Can be used to export the chart as a static image and returns a binary string of the exported image.
    /// </summary>
    /// <param name="chartDefinition">The chart definition to be exported.</param>
    /// <param name="format">Format of the image.</param>
    /// <param name="height">Height of the image.</param>
    /// <param name="width">Width of the image.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Binary string of the exported image.</returns>
    public async Task<string> ToImage(ChartDefinition chartDefinition, ImageFormat format, uint height, uint width, CancellationToken cancellationToken)
    {
        var jsRuntime = await moduleTask.Value;

        return await jsRuntime.InvokeAsync<string>("toImageFromChartData", cancellationToken, chartDefinition.PrepareJsInterop(SerializerOptions), format, height, width);
    }
}