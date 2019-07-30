using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.HTTP
{
    /// <summary>
    /// A parser that parses HTTP headers from a string
    /// </summary>
    public static class HeadersParser
    {
        public static Dictionary<string, string> Parse(string hstring)
        {
            var hdict = new Dictionary<string, string>();
            var hl = hstring.Split('\n');
            foreach(var l in hl)
            {
                var fl = l.Trim();
                if (fl == "") continue;
                if (!fl.Contains(":")) continue;
                string[] hs = fl.Split(':');
                string k = hs[0].Trim();
                string v = fl.Substring(fl.IndexOf(':') + 1).Trim();
                hdict[k] = v;
            }
            return hdict;
        }
    }
}
