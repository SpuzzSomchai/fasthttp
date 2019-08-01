using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Extensibility
{
    /// <summary>
    /// A struct representing the extension information.
    /// </summary>
    public struct ExtensionInfo
    {
        /// <summary>
        /// Gets or sets the name of the extension.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of the extension.
        /// </summary>
        public float Version { get; set; }

        /// <summary>
        /// Gets or sets the author of the extension.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the website of the extension.
        /// </summary>
        public string Website { get; set; }
    }
}
