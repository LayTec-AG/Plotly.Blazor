# Plotly.Blazor 
[![Build Status](https://img.shields.io/github/workflow/status/LayTec-AG/Plotly.Blazor/Build?label=build)](https://www.nuget.org/packages/Plotly.Blazor/) [![Publish Status](https://img.shields.io/github/workflow/status/LayTec-AG/Plotly.Blazor/Publish?label=publish)](https://www.nuget.org/packages/Plotly.Blazor/) [![Examples Status](https://img.shields.io/github/workflow/status/LayTec-AG/Plotly.Blazor/Examples?label=examples)](https://plotly-blazor.azurewebsites.net/) [![NuGet Status](https://img.shields.io/nuget/v/Plotly.Blazor)](https://www.nuget.org/packages/Plotly.Blazor/)

This library packages the well-known charting library [plotly.js](https://github.com/plotly/plotly.js) into a Razor component that can be used in a Blazor project.
The advantage of this wrapper is that the plotly scheme itself is used to generate the classes. So you can automatically update to the latest plotly.js version with the help of the generator.


## Getting Started
### Prerequisites

To create Blazor Server Apps, install the latest version of Visual Studio 2019 with the ASP.NET and web development workload.
For Blazor WebAssembly you need at least Visual Studio 2019 16.6+.
Another alternative would be to use Visual Studio code. Click [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio-code) for more information.

**Plotly.Blazor with version >= 2.0.0 requires .NET 6 or higher.**


### Installing

After you have created your Blazor project, you need to do the following steps:


**Install the latest NuGet Package**

Using Package Manager
```
Install-Package Plotly.Blazor
```

Using .NET CLI
```
dotnet add package Plotly.Blazor
```


**Add the following lines to your index.html or _Host.cshtml below or above the blazor.webassembly.js**

Info: *These files are already included in the NuGet Package!*

```
<!-- Import the plotly.js library -->
<script src="_content/Plotly.Blazor/plotly-latest.min.js" type="text/javascript"></script>

<!-- Import the plotly.js interop functions -->
<script src="_content/Plotly.Blazor/plotly-interop.js" type="text/javascript"></script>
```

**Add the following lines to your _Imports.razor**

```
@using Plotly.Blazor
@using Plotly.Blazor.Traces
```

**Now we're ready to go! :tada:**

### Usage

**Create the Razor component**

Info: *The chart reference is important so that we can update the chart later.*

```
<PlotlyChart @bind-Config="config" @bind-Layout="layout" @bind-Data="data" @ref="chart"/>
```

**Generate some initial data for your plot.**

```
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

```
private async Task AddData(int count = 100)
{
    if (!(chart.Data.FirstOrDefault() is Scatter scatter)) return;
    var (x, y) = Helper.GenerateData(scatter.X.Count + 1, scatter.X.Count + 1 + count);

    await chart.ExtendTrace(x, y, data.IndexOf(scatter));
}
```

## Examples

[Here](https://plotly-blazor.azurewebsites.net/) you can find a running instance of the [examples](Plotly.Blazor.Examples/). This is always up-to-date with the current state of the develop branch.

**What it might look like!**

![Image of Example](https://i.imgur.com/WU4tdSA.png)

## Known issues
### Performance
- Blazor WebAssembly is (currently) not intended for performance purposes! We therefore recommend Blazor Server.
This issue is tracked [here](https://github.com/dotnet/aspnetcore/issues/5466).
- IJSRuntime (currently) does not allow to adjust the serialization of objects. Accordingly, a conversion is carried out beforehand, which consumes a comparatively large amount of time. You can find this issue [here](https://github.com/dotnet/aspnetcore/issues/12685).

### Missing Implementations
- Events
- Add multiple traces with one call
- Delete multiple traces with one call
- plotly.animate
- plotly.addFrames
- <s>plotly.downloadImage</s>
- plotly.moveTraces
- <s>plotly.relayout</s> Thanks to [PeopleCanFly](https://github.com/Peop1eCanF1y)!
- <s>plotly.restyle</s> Thanks to [PeopleCanFly](https://github.com/Peop1eCanF1y)!
- <s>plotly.toImage</s>

## Versioning

We implement [SemVer](http://semver.org/) using [GitVersion](https://github.com/GitTools/GitVersion/)

## Authors

* **Sean McLeish** - *Initial work* - [sean-mcleish](https://github.com/sean-mcleish)
* **PeopleCanFly** - *Contributor* - [PeopleCanFly](https://github.com/Peop1eCanF1y)
* **Matthew R. Johnson** - *Contributor* - [Matthew R. Johnson](https://github.com/centreboard)


See also the list of [contributors](https://github.com/LayTec-AG/Plotly.Blazor/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
