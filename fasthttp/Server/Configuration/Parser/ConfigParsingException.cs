using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// The super class all configuration parsing related exceptions inherit from
    /// </summary>
    public class ConfigParsingException : Exception
    {
        public int LineNumber { get; set; } = -1;
    }
}
