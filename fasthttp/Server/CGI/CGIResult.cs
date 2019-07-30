using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.CGI
{
    /// <summary>
    /// A struct representing a CGI result from a CGI program
    /// </summary>
    public struct CGIResult
    {
        public string OutDocument { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
