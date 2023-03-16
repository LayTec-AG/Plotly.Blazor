using Plotly.Blazor.LayoutLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotly.Blazor.Interop
{

    /// <summary>
    ///     This class is used to parse the event data from the relayout jsinterop action.
    /// </summary>
    public class RelayoutEventData
    {
        /// <summary>
        ///     The x-axis of the layout. [x0, x1]. This may not be set.
        /// </summary>
        /// <remarks>
        ///     Array of objects to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </remarks>
        public object[] XRange { get; set; }


        /// <summary>
        ///     The y-axis of the layout. [y0, y1]. This may not be set.
        /// </summary>
        /// <remarks>
        ///     Array of objects to be compatible to multiple data types.
        ///     Has to be casted manually.
        /// </remarks>
        public object[] YRange { get; set; }
    }
}
