namespace Plotly.Blazor.Generator.Schema.Definitions
{
    /// <summary>
    ///     Class ValObject.
    /// </summary>
    public class ValObject
    {
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the required opts.
        /// </summary>
        /// <value>The required opts.</value>
        public string[] RequiredOpts { get; set; }

        /// <summary>
        ///     Gets or sets the other opts.
        /// </summary>
        /// <value>The other opts.</value>
        public string[] OtherOpts { get; set; }
    }
}