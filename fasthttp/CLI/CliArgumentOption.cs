using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// Represents a command line argument option (e.g. --help)
    /// </summary>
    public struct CliArgumentOption
    {
        /// <summary>
        /// The prefix used for CLI options
        /// </summary>
        public const string CliOptionPrefix = "--";

        /// <summary>
        /// The name of the command line argument option
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A property used to specify the type of data expected from the option
        /// </summary>
        public CliArgumentOptionValueType ExpectedValueType { get; set; }
    }
}
