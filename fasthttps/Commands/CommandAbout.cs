using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI.Server.Commands
{
    /// <summary>
    /// The CLI command for displaying application information
    /// </summary>
    public class CommandAbout : ICommand
    {
        public CommandResult ExecuteCommand(string[] args)
        {
            Console.WriteLine("FastHTTP HTTP Server\nVersion 1.0-debug-anycpu\nCopyright (C) mr_chainman (techspider) 2019.");
            return new CommandResult
            {
                ExitCode = 0,
                Message = "",
                State = null
            };
        }

        public void GetHelp(string locale)
        {
            Console.WriteLine(@"fasthttp --version - Display the version page for FastHTTP.");
        }

        public string GetName()
        {
            return "--version";
        }
    }
}
