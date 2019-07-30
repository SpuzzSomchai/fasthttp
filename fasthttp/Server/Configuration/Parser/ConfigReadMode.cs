using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    public enum ConfigReadMode
    {
        Normal,
        MultiLineComment,
        SectionDef
    }
}
