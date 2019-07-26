using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// A class containing the value of a CLI argument option
    /// </summary>
    public struct CliArgumentOptionValue
    {
        public CliArgumentOptionValueType Type { get; set; }
        public object Value { get; set; }
    }
}
