using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server
{
    /// <summary>
    /// A class representing the configuration of the Web Server
    /// </summary>
    public class ServerConfiguration
    {
        public bool EnableHTTPS { get; set; } = false;
        public int HttpPort { get; set; } = 80;
        public int HttpsPort { get; set; } = 443;
        public bool DisableLogging { get; set; } = false;
        public bool DumpRequests { get; set; } = false;
        public bool DumpRequestURLs { get; set; } = false;
        public int Threads { get; set; } = 1;
        public bool SinglePageMode { get; set; } = false;
        public string SinglePagePath { get; set; } = "";
        public string WWWFolder { get; set; } = "";
        public string[] IndexPrefixes { get; set; } = new string[] { ".html", ".htm" };
    }
}
