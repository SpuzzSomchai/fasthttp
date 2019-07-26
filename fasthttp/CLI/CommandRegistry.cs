using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// A class for storing console commands
    /// </summary>
    public class CommandRegistry
    {
        private List<ICommand> commands;

        /// <summary>
        /// Creates a new command registry with the specified commands
        /// </summary>
        /// <param name="commands">The array of commands to add</param>
        public CommandRegistry(params ICommand[] commands)
        {
            this.commands = commands.ToList();
        }

        /// <summary>
        /// Registers a new command and adds it to the registry
        /// </summary>
        /// <param name="cmd">The command to add</param>
        public void RegisterCommand(ICommand cmd)
        {
            this.commands.Add(cmd);
        }

        /// <summary>
        /// Executes a command by name
        /// </summary>
        /// <param name="name">The name of the command to locate</param>
        public CommandResult ExecuteCommandByName(string name, string[] args)
        {
            IEnumerable<ICommand> cmds = this.commands.Where(x => x.GetName() == name);
            if (cmds.Count() == 0)
                return new CommandResult { ExitCode = 255, Message = "", State = null };
            else return cmds.ElementAt(0).ExecuteCommand(args);
        }
    }
}
