using FastHTTP.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_extension
{
    public class TestRESTAPI : FastHTTP.Server.REST.RestAPI
    {
        public override string URL { get; set; } = "/restapi/testextension";

        public TestRESTAPI()
        {
            ResponseReceived += TestRESTAPI_ResponseReceived;
        }

        private void TestRESTAPI_ResponseReceived(System.Net.HttpListenerRequest req, System.Net.HttpListenerResponse resp, FastHTTP.Logging.Logger log)
        {
            log.Log(LogLevel.INFORMATION, "RestApiTest: Got request, responding");
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("{\"success\": \"true\"}"));
            ms.CopyTo(resp.OutputStream);
            ms.Close();
            resp.Close();
        }
    }
}
