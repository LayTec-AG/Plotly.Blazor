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
        ///     The x-axis of the layout. [x0, x1]
        /// </summary>
        /// <remarks>
        ///     In some cases this may be not be set.
        /// </remarks>
        public double[] XRange { get; set; }


        /// <summary>
        ///     The y-axis of the layout. [y0, y1].
        /// </summary>
        /// <remarks>
        ///      In some cases this may be not be set.
        /// </remarks>
        public double[] YRange { get; set; }
    }
}
