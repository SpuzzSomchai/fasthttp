using FastHTTP.CLI.Server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI.Server
{
    public class FastHTTPServer
    {
        public static void Main(string[] args)
        {
            /* TODO use command argument processor for every command so we can figure out what the user wants to do
            CliArgumentProcessor argumentProcessor = new CliArgumentProcessor(args, new CliArgumentOption() { Name = "port", ExpectedValueType = CliArgumentOptionValueType.Integer }, 
                new CliArgumentOption { Name = "yes", ExpectedValueType = CliArgumentOptionValueType.None });
            argumentProcessor.CatchUnknownOptions = true;
            argumentProcessor.Error += ArgumentProcessor_Error;
            argumentProcessor.ParseArguments();*/
            CommandRegistry reg = new CommandRegistry(new CommandAbout(), new CommandHelp(), new CommandStart());
            if(args.Length < 1)
            {
                ConsoleExtensions.WriteLineError("Error: No command specified, run \"{0}\" for a list of available commands.", "fasthttps --help");
                return;
            }
            CommandResult res = reg.ExecuteCommandByName(args[0], args);
            if(res.ExitCode == 255)
            {
                ConsoleExtensions.WriteLineError("Error: \"{0}\" is not a valid command, try \"fasthttps --help\" for help.", args[0]);
            }
            Environment.Exit(res.ExitCode);
        }

        public static void ArgumentProcessor_Error(string optionName, string value, CliArgumentProcessorErrorType errorType)
        {
            switch (errorType)
            {
                case CliArgumentProcessorErrorType.IntegerExpected:
                    ConsoleExtensions.WriteLineError("Error: Integer expected for option \"{0}\" but got value \"{1}\".", optionName, value);
                    break;
                case CliArgumentProcessorErrorType.UnknownOption:
                    ConsoleExtensions.WriteLineError("Error: Option \"{0}\" is not a valid option!", optionName);
                    break;
                case CliArgumentProcessorErrorType.ValueExpected:
                    ConsoleExtensions.WriteLineError("Error: A value was expected for option \"{0}\"", optionName);
                    break;
            }
            Environment.Exit(1);
        }
    }
}
