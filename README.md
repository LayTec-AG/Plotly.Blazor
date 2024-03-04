# Plotly.Blazor

![Build Status](https://img.shields.io/github/actions/workflow/status/LayTec-AG/Plotly.Blazor/build.yml?branch=main&label=Build)
[![Examples Status](https://img.shields.io/github/actions/workflow/status/LayTec-AG/Plotly.Blazor/pages%2Fpages-build-deployment?label=Examples)](https://laytec-ag.github.io/Plotly.Blazor)
[![NuGet Status](https://img.shields.io/nuget/v/Plotly.Blazor)](https://www.nuget.org/packages/Plotly.Blazor/) 
[![Forks](https://img.shields.io/github/forks/LayTec-AG/Plotly.Blazor)](https://github.com/LayTec-AG/Plotly.Blazor/network/members)
![Stars](https://img.shields.io/github/stars/LayTec-AG/Plotly.Blazor)
![License](https://img.shields.io/github/license/LayTec-AG/Plotly.Blazor)

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
```powershell
Install-Package Plotly.Blazor
```

Using .NET CLI
```cmd
dotnet add package Plotly.Blazor
```

**If you are using 4.1.0 or lower**, then add the following lines to your _Layout.cshtml **above** the `_/framework/blazor.webassembly.js` or `_/framework/blazor.server.js`

```razor
<script src="_content/Plotly.Blazor/plotly-latest.min.js" type="text/javascript"></script>
<script src="_content/Plotly.Blazor/plotly-interop.js" type="text/javascript"></script>
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

## Contributors

<table>
<tr>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/sean-mcl>
            <img src=https://avatars.githubusercontent.com/u/64470948?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Sean McLeish/>
            <br />
            <sub style="font-size:14px"><b>Sean McLeish</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/guido-altmann>
            <img src=https://avatars.githubusercontent.com/u/61827902?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Guido Altmann/>
            <br />
            <sub style="font-size:14px"><b>Guido Altmann</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/centreboard>
            <img src=https://avatars.githubusercontent.com/u/10487726?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Matthew R Johnson/>
            <br />
            <sub style="font-size:14px"><b>Matthew R Johnson</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/yaqian256>
            <img src=https://avatars.githubusercontent.com/u/83148975?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=yaqian256/>
            <br />
            <sub style="font-size:14px"><b>yaqian256</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/jvaque>
            <img src=https://avatars.githubusercontent.com/u/32816191?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=Jesus Vaquerizo/>
            <br />
            <sub style="font-size:14px"><b>Jesus Vaquerizo</b></sub>
        </a>
    </td>
    <td align="center" style="word-wrap: break-word; width: 150.0; height: 150.0">
        <a href=https://github.com/parnold75>
            <img src=https://avatars.githubusercontent.com/u/11851030?v=4 width="100;"  style="border-radius:50%;align-items:center;justify-content:center;overflow:hidden;padding-top:10px" alt=parnold75/>
            <br />
            <sub style="font-size:14px"><b>parnold75</b></sub>
        </a>
    </td>
</tr>
</table>

## Versioning

We implement [SemVer](http://semver.org/) using [GitVersion](https://github.com/GitTools/GitVersion/)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
