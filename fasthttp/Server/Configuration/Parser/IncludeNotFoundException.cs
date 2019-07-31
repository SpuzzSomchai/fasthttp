using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    public class IncludeNotFoundException : ConfigParsingException
    {
        public string IncludePath { get; set; }
        public override string Message
        {
            get
            {
                return string.Format("Error at ln {1}: Cannot find include \"{0}\"", IncludePath, LineNumber);
            }
        }

        public IncludeNotFoundException(string includePath, int lineNo)
        {
            IncludePath = includePath;
            LineNumber = lineNo;
        }
    }
}
