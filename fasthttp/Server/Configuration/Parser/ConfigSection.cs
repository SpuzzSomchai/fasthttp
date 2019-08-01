using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.Configuration.Parser
{
    /// <summary>
    /// A configuration section
    /// </summary>
    public class ConfigSection
    {
        /// <summary>
        /// A dictionary containing all variables which belong to the specified section
        /// </summary>
        public Dictionary<string, ConfigVariable> Variables { get; set; } = new Dictionary<string, ConfigVariable>();

        /// <summary>
        /// Returns a variable by its name
        /// </summary>
        /// <param name="name">The variable name to find</param>
        /// <returns></returns>
        public ConfigVariable GetVariableByName(string name)
        {
            if (!Variables.ContainsKey(name)) return null;
            else return Variables[name];
        }
    }
}
