using System;
using System.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Plotly.Blazor.Examples.Shared
{
    public partial class NavMenu
    {
        private bool open = true;

        [Inject] private NavigationManager NavManager { get; set; }

        private void OpenOrCloseDrawer()
        {
            open = !open;
        }

    }
}