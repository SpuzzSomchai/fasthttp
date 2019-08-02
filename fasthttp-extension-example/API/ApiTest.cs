using FastHTTP.Logging;
using FastHTTP.Server.REST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fasthttp_extension_example.API
{
    /// <summary>
    /// An example of a REST API.
    /// </summary>
    public class ApiTest : RestAPI
    {
        /// <summary>
        /// The URL of the API.
        /// </summary>
        public override string URL { get; set; } = "/api/test";

        /// <summary>
        /// The constructor of the API. Use this to assign events, etc.
        /// </summary>
        public ApiTest()
        {
            ResponseReceived += TestRESTAPI_ResponseReceived;
        }

        /// <summary>
        /// A void function called whenever a response has been received.
        /// </summary>
        /// <param name="req">The request object.</param>
        /// <param name="resp">The response object.</param>
        /// <param name="log">A logger representing the log.</param>
        private void TestRESTAPI_ResponseReceived(System.Net.HttpListenerRequest req, System.Net.HttpListenerResponse resp, Logger log)
        {
            log.Log(LogLevel.INFORMATION, "REST ApiTest: Got request, responding");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("{\"success\": \"true\"}"));
            ms.CopyTo(resp.OutputStream);
            ms.Close();
            resp.Close();
        }
    }
}
