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
    /// The CLI command used to start up the web server
    /// </summary>
    public class CommandStart : ICommand
    {
        public CommandResult ExecuteCommand(string[] args)
        {
            CliArgumentProcessor argumentProcessor = new CliArgumentProcessor(args, new CliArgumentOption() { Name = "port", ExpectedValueType = CliArgumentOptionValueType.Integer }, 
                new CliArgumentOption { Name = "help", ExpectedValueType = CliArgumentOptionValueType.None });
            argumentProcessor.CatchUnknownOptions = true;
            argumentProcessor.Error += FastHTTPServer.ArgumentProcessor_Error;
            argumentProcessor.ParseArguments();
            if(argumentProcessor.HasOption("help"))
            {
                GetHelp("default");
                return new CommandResult { ExitCode = 0, Message = "", State = null };
            }
            return new CommandResult { ExitCode = 0, Message = "", State = null };
        }

        public void GetHelp(string locale)
        {
            Console.WriteLine("Usage: fasthttps start [flags]");
            Console.WriteLine(@"Flags:
  --disable-log              - Disables logging (does not affect --dump-req-urls and --dump-reqs flag)
  --dump-reqs                - Dumps all received request bodies to tmp/[url].[content-type-mime]
  --dump-req-urls            - Dumps all received request URLs to tmp/reqs.txt
  --enable-https             - Enables communication over HTTPS (including HTTP)
  --port <int 1-65535>       - Sets a custom port for the HTTP server
  --port-https <int 1-65535> - Sets a custom port for the HTTPS server
  --single-page <path>       - Responds with a single page to every request (disables www root dir)
  --working-dir <path>       - Sets the working directory for the web server (defaults to [current dir]/www)");
        }

        public string GetName()
        {
            return "start";
        }
    }
}
