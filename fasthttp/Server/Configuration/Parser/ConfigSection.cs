using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// A configuration section
    /// </summary>
    public class ConfigSection
    {
        public Dictionary<string, ConfigVariable> Variables { get; set; }
    }
}
