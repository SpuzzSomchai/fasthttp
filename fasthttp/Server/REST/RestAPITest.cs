using FastHTTP.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.REST
{
    /// <summary>
    /// A test class for RestAPI
    /// </summary>
    public class RestAPITest : RestAPI
    {
        public override string URL { get; set; } = "/restapi/test";

        public RestAPITest()
        {
            ResponseReceived += RestAPITest_ResponseReceived;
        }

        private void RestAPITest_ResponseReceived(System.Net.HttpListenerRequest req, System.Net.HttpListenerResponse resp, Logger log)
        {
            log.Log(LogLevel.INFORMATION, "RestApiTest: Got request, responding");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("{\"success\": \"true\"}"));
            ms.CopyTo(resp.OutputStream);
            ms.Close();
            resp.Close();
        }
    }
}
