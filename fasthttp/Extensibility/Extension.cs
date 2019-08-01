using FastHTTP.Server;
using FastHTTP.Server.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Extensibility
{
    /// <summary>
    /// The base class for an extension
    /// </summary>
    public abstract class Extension
    {
        public abstract ExtensionInfo VersionInformation { get; set; }
        public abstract void OnEnabled(ServerExecutionContext context);
        public abstract void OnDisabled();
        public abstract RestAPI[] GetRestAPIs();
    }
}
