using FastHTTP.Extensibility;
using FastHTTP.Server;
using FastHTTP.Server.REST;
using fasthttp_extension_example.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fasthttp_extension_example
{
    /// <summary>
    /// The main class representing the extension.
    /// </summary>
    public class ExampleExtension : Extension
    {
        /// <summary>
        /// A private variable referencing the current server context.
        /// </summary>
        private ServerExecutionContext sctx;

        /// <summary>
        /// A property used to return the version information of the extension.
        /// </summary>
        public override ExtensionInfo VersionInformation { get; set; } = new ExtensionInfo
        {
            Author = "Some author",
            Name = "Example Extension",
            Version = 1.0f,
            Website = "http://example.com"
        };

        /// <summary>
        /// A function to return all available REST APIs.
        /// </summary>
        /// <returns></returns>
        public override RestAPI[] GetRestAPIs()
        {
            return new RestAPI[] { new ApiTest() };
        }

        /// <summary>
        /// Called when the extension is about to be unloaded.
        /// </summary>
        public override void OnDisabled()
        {
            sctx.ServerLog.Log(FastHTTP.Logging.LogLevel.INFORMATION, "Goodbye from {0}", VersionInformation.Name);
        }

        /// <summary>
        /// Called when the extension has been enabled.
        /// </summary>
        /// <param name="context">The current server context.</param>
        public override void OnEnabled(ServerExecutionContext context)
        {
            sctx = context;
            sctx.ServerLog.Log(FastHTTP.Logging.LogLevel.INFORMATION, "{0} has started!", VersionInformation.Name);
        }
    }
}
