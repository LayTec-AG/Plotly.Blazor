using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.JSInterop;
using NUnit.Framework;

namespace Plotly.Blazor.Tests;

public class DisposalTests
{
    [Test]
    public void PlotlyJsInterop_Dispose_DisposesDotNetReference()
    {
        var chart = new PlotlyChart();

        var interop = new PlotlyJsInterop(new TestJsRuntime(), chart, false);
        var dotNetObjectReference = GetDotNetObjectReference(interop);

        GetDisposed(dotNetObjectReference).Should().BeFalse();

        interop.Dispose();

        GetDisposed(dotNetObjectReference).Should().BeTrue();
    }

    [Test]
    public void PlotlyChart_Dispose_DelegatesToInterop()
    {
        var chart = new PlotlyChart();
        var jsRuntime = new TestJsRuntime();

        SetPrivateProperty(chart, "JsRuntime", jsRuntime);
        InvokeNonPublic(chart, "OnInitialized");

        var interop = GetPrivateProperty<PlotlyJsInterop>(chart, "Interop");
        var dotNetObjectReference = GetDotNetObjectReference(interop);

        interop.React(CancellationToken.None).GetAwaiter().GetResult();
        GetDisposed(dotNetObjectReference).Should().BeFalse();

        chart.Dispose();

        GetDisposed(dotNetObjectReference).Should().BeTrue();
        jsRuntime.Module.DisposeCalled.Should().BeTrue();
    }

    [Test]
    public async Task PlotlyJsInterop_DisposeAsync_IsFailSafe_WhenModuleInitializationFailed()
    {
        var chart = new PlotlyChart();

        var interop = new PlotlyJsInterop(new TestJsRuntime
        {
            ImportException = new InvalidOperationException("import failed")
        }, chart, false);

        var dotNetObjectReference = GetDotNetObjectReference(interop);

        await Assert.ThatAsync(() => interop.React(CancellationToken.None), Throws.TypeOf<InvalidOperationException>());
        await Assert.ThatAsync(() => interop.DisposeAsync().AsTask(), Throws.Nothing);
        GetDisposed(dotNetObjectReference).Should().BeTrue();
    }

    [Test]
    public async Task PlotlyJsInterop_DisposeAsync_IsFailSafe_WhenJsCleanupThrows()
    {
        var chart = new PlotlyChart();

        var jsRuntime = new TestJsRuntime
        {
            Module = new()
            {
                ThrowOnPurge = true
            }
        };

        var interop = new PlotlyJsInterop(jsRuntime, chart, false);
        var dotNetObjectReference = GetDotNetObjectReference(interop);

        await interop.React(CancellationToken.None);
        await Assert.ThatAsync(() => interop.DisposeAsync().AsTask(), Throws.Nothing);
        GetDisposed(dotNetObjectReference).Should().BeTrue();
        jsRuntime.Module.DisposeCalled.Should().BeTrue();
    }

    private static object GetDotNetObjectReference(PlotlyJsInterop interop)
    {
        return typeof(PlotlyJsInterop)
                   .GetField("dotNetObj", BindingFlags.Instance | BindingFlags.NonPublic)
                   ?.GetValue(interop)
               ?? throw new InvalidOperationException("dotNetObj field not found.");
    }

    private static bool GetDisposed(object dotNetObjectReference)
    {
        return (bool)(dotNetObjectReference.GetType()
                          .GetProperty("Disposed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                          ?.GetValue(dotNetObjectReference)
                      ?? throw new InvalidOperationException("Disposed property not found."));
    }

    private static T GetPrivateProperty<T>(object instance, string propertyName)
    {
        return (T)(instance.GetType()
                       .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                       ?.GetValue(instance)
                   ?? throw new InvalidOperationException($"Property '{propertyName}' not found."));
    }

    private static void SetPrivateProperty(object instance, string propertyName, object value)
    {
        var property = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                       ?? throw new InvalidOperationException($"Property '{propertyName}' not found.");

        property.SetValue(instance, value);
    }

    private static void InvokeNonPublic(object instance, string methodName)
    {
        var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)
                     ?? throw new InvalidOperationException($"Method '{methodName}' not found.");

        method.Invoke(instance, null);
    }

    private sealed class TestJsRuntime : IJSRuntime
    {
        public Exception ImportException { get; init; }

        public TestJsObjectReference Module { get; init; } = new();

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
            object[] args)
        {
            if (identifier != "import")
            {
                throw new InvalidOperationException($"Unexpected identifier '{identifier}'.");
            }

            return ImportException is not null
                ? throw ImportException
                : new((TValue)(object)Module);
        }
    }

    private sealed class TestJsObjectReference : IJSObjectReference
    {
        public bool ThrowOnPurge { get; init; }

        public bool ThrowOnDispose { get; init; }

        public bool DisposeCalled { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeCalled = true;

            return ThrowOnDispose
                ? throw new InvalidOperationException("dispose failed")
                : ValueTask.CompletedTask;
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            return InvokeAsync<TValue>(identifier, CancellationToken.None, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
            object[] args)
        {
            if (identifier == "purge" && ThrowOnPurge)
            {
                throw new InvalidOperationException("purge failed");
            }

            return identifier is "react" or "importScript" or "purge"
                ? new(default(TValue))
                : throw new InvalidOperationException($"Unexpected identifier '{identifier}'.");
        }
    }
}