using FastHTTP.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server
{
    /// <summary>
    /// A class representing the current execution context for FastHTTPS
    /// </summary>
    public class ServerExecutionContext
    {
        public ServerConfiguration Configuration { get; set; }
        public HTTPServer Server { get; set; }
        public Logger ServerLog { get; set; }
    }
}
