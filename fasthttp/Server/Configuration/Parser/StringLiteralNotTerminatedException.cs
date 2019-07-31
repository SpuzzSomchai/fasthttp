using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// An exception thrown when a string literal has not been correctly terminated.
    /// </summary>
    public class StringLiteralNotTerminatedException : ConfigParsingException
    {
        public string VariableName { get; set; }
        public override string Message
        {
            get
            {
                return string.Format("Error at ln {1}: String literal for variable \"{0}\" has not been correctly terminated", VariableName, LineNumber);
            }
        }

        public StringLiteralNotTerminatedException(string varName, int lineNumber)
        {
            VariableName = varName;
            LineNumber = lineNumber;
        }
    }
}
