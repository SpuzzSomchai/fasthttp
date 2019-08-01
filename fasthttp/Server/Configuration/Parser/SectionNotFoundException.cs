using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    public class SectionNotFoundException : ConfigParsingException
    {
        public string Path { get; set; }

        public SectionNotFoundException(string path)
        {
            Path = path;
        }

        public override string Message
        {
            get
            {
                return string.Format("The section path {0} is not found!", Path);
            }
        }
    }
}
