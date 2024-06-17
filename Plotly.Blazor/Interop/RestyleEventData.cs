using System.Collections.Generic;
using System.Text.Json;

namespace Plotly.Blazor.Interop
{
    /// <summary>
    ///     This class is used to parse the event data from the restyle action.
    /// </summary>
    public class RestyleEventData
    {
        /// <summary>
        ///     Returns a dictionary of all updated properties including their new value.
        /// </summary>
        public IDictionary<string, JsonElement> Changes { get; set; }

        /// <summary>
        ///     The updated trace indices with the given <see cref="Changes"/>.
        /// </summary>
        public int[] Indices { get; set; }
    }
}