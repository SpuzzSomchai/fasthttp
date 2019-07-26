using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// Represents the types of Errors that could be thrown during CLI argument processing
    /// </summary>
    public enum CliArgumentProcessorErrorType
    {
        IntegerExpected,
        ValueExpected,
        UnknownOption
    }
}
