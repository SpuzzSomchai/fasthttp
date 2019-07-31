using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// Simple exception thrown when a constant is already defined
    /// </summary>
    public class ConstantAlreadyDefinedException : ConfigParsingException
    {
        public string ConstantName { get; set; } = "None";

        public override string Message
        {
            get
            {
                //TODO localize exception
                return string.Format("Error at ln {0}: The constant \"{0}\" has already been defined!", ConstantName, LineNumber);
            }
        }

        public ConstantAlreadyDefinedException(string constantName, int lineNumber)
        {
            ConstantName = constantName;
            LineNumber = lineNumber;
        }
    }
}
