# Plotly.Blazor

![Build Status](https://img.shields.io/github/actions/workflow/status/LayTec-AG/Plotly.Blazor/build.yml?branch=main&label=Build)
[![Examples Status](https://img.shields.io/github/actions/workflow/status/LayTec-AG/Plotly.Blazor/pages%2Fpages-build-deployment?label=Examples)](https://laytec-ag.github.io/Plotly.Blazor)
[![DeepWiki](https://img.shields.io/badge/DeepWiki-LayTec--AG%2FPlotly.Blazor-blue.svg?logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACwAAAAyCAYAAAAnWDnqAAAAAXNSR0IArs4c6QAAA05JREFUaEPtmUtyEzEQhtWTQyQLHNak2AB7ZnyXZMEjXMGeK/AIi+QuHrMnbChYY7MIh8g01fJoopFb0uhhEqqcbWTp06/uv1saEDv4O3n3dV60RfP947Mm9/SQc0ICFQgzfc4CYZoTPAswgSJCCUJUnAAoRHOAUOcATwbmVLWdGoH//PB8mnKqScAhsD0kYP3j/Yt5LPQe2KvcXmGvRHcDnpxfL2zOYJ1mFwrryWTz0advv1Ut4CJgf5uhDuDj5eUcAUoahrdY/56ebRWeraTjMt/00Sh3UDtjgHtQNHwcRGOC98BJEAEymycmYcWwOprTgcB6VZ5JK5TAJ+fXGLBm3FDAmn6oPPjR4rKCAoJCal2eAiQp2x0vxTPB3ALO2CRkwmDy5WohzBDwSEFKRwPbknEggCPB/imwrycgxX2NzoMCHhPkDwqYMr9tRcP5qNrMZHkVnOjRMWwLCcr8ohBVb1OMjxLwGCvjTikrsBOiA6fNyCrm8V1rP93iVPpwaE+gO0SsWmPiXB+jikdf6SizrT5qKasx5j8ABbHpFTx+vFXp9EnYQmLx02h1QTTrl6eDqxLnGjporxl3NL3agEvXdT0WmEost648sQOYAeJS9Q7bfUVoMGnjo4AZdUMQku50McDcMWcBPvr0SzbTAFDfvJqwLzgxwATnCgnp4wDl6Aa+Ax283gghmj+vj7feE2KBBRMW3FzOpLOADl0Isb5587h/U4gGvkt5v60Z1VLG8BhYjbzRwyQZemwAd6cCR5/XFWLYZRIMpX39AR0tjaGGiGzLVyhse5C9RKC6ai42ppWPKiBagOvaYk8lO7DajerabOZP46Lby5wKjw1HCRx7p9sVMOWGzb/vA1hwiWc6jm3MvQDTogQkiqIhJV0nBQBTU+3okKCFDy9WwferkHjtxib7t3xIUQtHxnIwtx4mpg26/HfwVNVDb4oI9RHmx5WGelRVlrtiw43zboCLaxv46AZeB3IlTkwouebTr1y2NjSpHz68WNFjHvupy3q8TFn3Hos2IAk4Ju5dCo8B3wP7VPr/FGaKiG+T+v+TQqIrOqMTL1VdWV1DdmcbO8KXBz6esmYWYKPwDL5b5FA1a0hwapHiom0r/cKaoqr+27/XcrS5UwSMbQAAAABJRU5ErkJggg==)](https://deepwiki.com/LayTec-AG/Plotly.Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Plotly.Blazor)](https://www.nuget.org/packages/Plotly.Blazor/) 
[![Forks](https://img.shields.io/github/forks/LayTec-AG/Plotly.Blazor)](https://github.com/LayTec-AG/Plotly.Blazor/network/members)
![Stars](https://img.shields.io/github/stars/LayTec-AG/Plotly.Blazor)
![License](https://img.shields.io/github/license/LayTec-AG/Plotly.Blazor)


This library packages the well-known charting library [plotly.js](https://github.com/plotly/plotly.js) into a Razor component that can be used in a Blazor project.
The advantage of this wrapper is that the plotly scheme itself is used to generate the classes. So you can automatically update to the latest plotly.js version with the help of the generator.

## Getting Started

### Prerequisites

To create Blazor Server Apps, install the latest version of Visual Studio with the ASP.NET and web development workload.
Another alternative would be to use Visual Studio code. Click [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio-code) for more information.

**Plotly.Blazor with version >= 7.0.0 requires .NET 8 or higher.**
**Plotly.Blazor with version >= 2.0.0 requires .NET 6 or higher.**


### Installing

After you have created your Blazor project, you need to do the following steps:


**Install the latest NuGet Package**

Using Package Manager
```powershell
Install-Package Plotly.Blazor
```

Using .NET CLI
```cmd
dotnet add package Plotly.Blazor
```

**Add the following lines to your _Imports.razor**

```razor
@using Plotly.Blazor
@using Plotly.Blazor.Traces
```

**Now we're ready to go! :tada:**

### Usage

**Create the Razor component**

Info: *The chart reference is important so that we can update the chart later.*

```razor
<PlotlyChart @bind-Config="config" @bind-Layout="layout" @bind-Data="data" @ref="chart"/>
```

**Generate some initial data for your plot.**

```razor
@code {
    PlotlyChart chart;
    Config config = new Config();
    Layout layout = new Layout();
    // Using of the interface IList is important for the event callback!
    IList<ITrace> data = new List<ITrace>
    {
        new Scatter
        {
            Name = "ScatterTrace",
            Mode = ModeFlag.Lines | ModeFlag.Markers,
            X = new List<object>{1,2,3},
            Y = new List<object>{1,2,3}
        }
    };
}
```

**Generate some additional data for your plot.**

```csharp
private async Task AddData(int count = 100)
{
    if (!(chart.Data.FirstOrDefault() is Scatter scatter)) return;
    var (x, y) = Helper.GenerateData(scatter.X.Count + 1, scatter.X.Count + 1 + count);

    await chart.ExtendTrace(x, y, data.IndexOf(scatter));
}
```

## Examples

[Here](https://laytec-ag.github.io/Plotly.Blazor) you can find a running instance of the [examples](Plotly.Blazor.Examples/). This is always up-to-date with the current state of the develop branch.

**What it might look like!**

![Image of Example](https://i.imgur.com/WU4tdSA.png)

## Missing Implementations

- Events
- Add multiple traces with one call
- Delete multiple traces with one call
- plotly.animate
- plotly.addFrames
- plotly.moveTraces

## Versioning

We implement [SemVer](http://semver.org/) using [GitVersion](https://github.com/GitTools/GitVersion/)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
