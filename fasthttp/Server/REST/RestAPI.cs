using FastHTTP.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.REST
{
    /// <summary>
    /// A class representing a REST API
    /// </summary>
    public abstract class RestAPI
    {
        public delegate void RestRequestEventHandler(HttpListenerRequest req, HttpListenerResponse resp, Logger log); //TODO abstract away HTTP.SYS listener requests/responses with something better
        public event RestRequestEventHandler ResponseReceived;
        public abstract string URL { get; set; }

        public void InvokeResponseReceived(HttpListenerRequest req, HttpListenerResponse resp, Logger log)
        {
            ResponseReceived?.Invoke(req, resp, log);
        }
    }
}
