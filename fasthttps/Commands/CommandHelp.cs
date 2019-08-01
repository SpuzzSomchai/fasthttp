using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FastHTTP.CLI.ConsoleExtensions;
using static System.ConsoleColor;

namespace FastHTTP.CLI.Server.Commands
{
    /// <summary>
    /// The CLI command for getting general help using FastHTTPServer
    /// </summary>
    public class CommandHelp : ICommand
    {
        public CommandResult ExecuteCommand(string[] args)
        {
            GetHelp("default");
            return new CommandResult { ExitCode = 0, Message = "", State = null };
        }

        public void GetHelp(string locale)
        {
            PrintColor(Yellow, "FastHTTP Server version 1.0 help page\n");
            Print("Usage: fasthttps <command> [flags] ...\nUsage: fasthttps [flags] ...\n");
            Print(@"Available Commands:
  about           - About FastHTTPServer.
  editcfg         - Launch notepad to edit the server configuration file.
  start [options] - Start the HTTP(s) server.

Flags:
  --help          - View the help page of <command> or FastHTTPServer itself.
  --version       - Get version information for FastHTTPServer.");
        }

        public string GetName()
        {
            return "--help";
        }
    }
}
