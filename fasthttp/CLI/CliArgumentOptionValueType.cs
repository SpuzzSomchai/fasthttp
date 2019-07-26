using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// An enum representing the different value types a CLI option can have
    /// </summary>
    public enum CliArgumentOptionValueType
    {
        Integer,
        String,
        CommaSeparatedList,
        None, //No value, default
        Null
    }
}
