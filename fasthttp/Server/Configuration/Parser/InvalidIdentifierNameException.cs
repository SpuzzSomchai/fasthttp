using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// An exception thrown whenever an identifier with an invalid name has been defined or used
    /// </summary>
    public class InvalidIdentifierNameException : ConfigParsingException
    {
        public string IdName { get; set; }

        public override string Message
        {
            get
            {
                return string.Format("Error at ln {0}: The identifier name \"{1}\" is invalid!", LineNumber, IdName);
            }
        }

        public InvalidIdentifierNameException(int lineNumber, string idName)
        {
            LineNumber = lineNumber;
            IdName = idName;
        }
    }
}
