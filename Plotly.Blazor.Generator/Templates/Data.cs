using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plotly.Blazor.Generator.Templates
{
    public abstract class Data
    {
        /// <summary>
        ///     Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the generator name.
        /// </summary>
        /// <value>The generator name.</value>
        public string GeneratorName => Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        ///     Gets the generator version.
        /// </summary>
        /// <value>The generator version.</value>
        public Version GeneratorVersion => Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public IEnumerable<string> Description { get; set; }

    }
}