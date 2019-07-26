using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// An object representing the result after command execution
    /// </summary>
    public struct CommandResult
    {
        public int ExitCode { get; set; }
        public string Message { get; set; }
        public object State { get; set; }
    }
}
