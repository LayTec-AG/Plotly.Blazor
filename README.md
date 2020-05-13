# Plotly.Blazor ![Build & Deploy](https://github.com/LayTec-AG/Plotly.Blazor/workflows/Build%20&%20Deploy/badge.svg)
This library packages the well-known charting library plotly.js into a component that can be used in a Blazor project. 

## Getting Started
### Prerequisites

To create Blazor Server Apps, install the latest version of Visual Studio 2019 with the ASP.NET and web development workload.
For the example with Blazor WebAssembly you need the current preview of Visual Studio 2019.
Another alternative would be to use Visual Studio code. Click [here](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio-code) for more information.

### Installing

After you have created your Blazor project, you need to do the following steps:


**Install the latest NuGet Package**

Using Package Manager
```
Install-Package Plotly.Blazor -Version 0.1.0-alpha.38
```

Using .NET CLI
```
dotnet add package Plotly.Blazor --version 0.1.0-alpha.38
```


**Add the following lines to your index.html or _Host.cshtml below the blazor.webassembly.js**

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
@using Plotly.Blazor.Common
@using Plotly.Blazor.Traces
@using Plotly.Blazor.Traces.Scatter
```

**Now we're ready to go! :tada:**

### Usage

**Create the Razor component**

Info: *The chart reference is important so that we can update the chart later.*

```
<PlotlyChart Id="TestId" Config="config" Layout="layout" Data="data" @ref="chart"/>
```

**Generate some initial data for your plot.**

```
@code {
    PlotlyChart chart;
    Config config = new Config();
    Layout layout = new Layout();
    List<Scatter> data = new List<Scatter>
    {
        new Scatter
        {
            Name = "Trace1",
            Mode = ScatterMode.Lines
        }.GenerateData(0, 100)
    };
}
```

**Generate some additional data for your plot.**

```
private async Task AddData(Scatter trace, int count = 100)
{
    var (x, y) = Helper.GenerateData(trace.X.Count + 1, trace.X.Count + 1 + count);

    trace.X.AddRange(x);
    trace.Y.AddRange(y);

    await chart.Update();
}
```

**What it might look like!  (example from the repository)**

![Image of Example](https://i.imgur.com/WU4tdSA.png)


## Contributing

The project is currently still in its initial state. Therefore, a contribution is currently not possible.

## Versioning

We implement [SemVer](http://semver.org/) using [GitVersion](https://github.com/GitTools/GitVersion/)

## Authors

* **Sean McLeish** - *Initial work* - [sean-mcleish](https://github.com/sean-mcleish)

See also the list of [contributors](https://github.com/LayTec-AG/Plotly.Blazor/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
