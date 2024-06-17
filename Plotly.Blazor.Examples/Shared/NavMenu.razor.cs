using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Plotly.Blazor.Examples.Shared
{
    public partial class NavMenu
    {
        private bool open = true;

        [Inject] private NavigationManager NavManager { get; set; }

        protected override void OnInitialized()
        {
            NavManager.LocationChanged += HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            NavManager.LocationChanged -= HandleLocationChanged;
        }

        private class PageInfo
        {
            public string Page { get; set; }
            public string Title { get; set; }
        }

        private List<PageInfo> pageInfos = new()
        {
            new() { Page = "", Title = "Scatter"},
            new() { Page = "scatter-3d", Title = "Scatter3D" },
            new() { Page = "live-data", Title = "Live Data" },
            new() { Page = "bar", Title = "Bar" },
            new() { Page = "pie", Title = "Pie" },
            new() { Page = "box", Title = "Box" },
            new() { Page = "candlestick", Title = "Candlestick" },
            new() { Page = "multipleaxes", Title = "Multiple Axes" },
            new() { Page = "shapes", Title = "Shapes" },
            new() { Page = "map", Title = "Map" },
            new() { Page = "surface", Title = "Surface" },
            new() { Page = "ribbon", Title = "Ribbon" },
            new() { Page = "heatmap", Title = "HeatMap" },
            new() { Page = "scattergl", Title = "ScatterGl" },
            new() { Page = "indicator", Title = "Indicator" },
            new() { Page = "smith", Title = "Smith" },
            new() { Page = "line-polar", Title = "Line Polar" },
            new() { Page = "area-polar", Title = "Area Polar" },
            new() { Page = "categorical-polar", Title = "Categorical Polar" },
            new() { Page = "direction-polar", Title = "Polar Direction" },
            new() { Page = "sector-polar", Title = "Polar Sector" },
            new() { Page = "hover", Title = "Hover Event" },
            new() { Page = "click", Title = "Click Event" },
            new() { Page = "legendclick", Title = "Legend Click Event" },
            new() { Page = "relayout", Title = "Relayout & Restyle Event" },
            new() { Page = "selected", Title = "Selected Event" }
        };

        private void OpenOrCloseDrawer()
        {
            open = !open;
        }

    }
}