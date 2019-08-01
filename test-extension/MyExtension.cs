using FastHTTP.Extensibility;
using FastHTTP.Server;
using FastHTTP.Server.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_extension
{
    public class MyExtension : Extension
    {
        private ServerExecutionContext sctx;

        public override ExtensionInfo VersionInformation { get; set; } = new ExtensionInfo
        {
            Author = "Some author",
            Name = "Test Extension",
            Version = 1.0f,
            Website = "http://example.com"
        };

        public override RestAPI[] GetRestAPIs()
        {
            return new RestAPI[] { new TestRESTAPI() };
        }

        public override void OnDisabled()
        {
            sctx.ServerLog.Log(FastHTTP.Logging.LogLevel.INFORMATION, "Goodbye from test extension");
        }

        public override void OnEnabled(ServerExecutionContext context)
        {
            sctx = context;
            sctx.ServerLog.Log(FastHTTP.Logging.LogLevel.INFORMATION, "{0} has started!", VersionInformation.Name);
        }
    }
}
