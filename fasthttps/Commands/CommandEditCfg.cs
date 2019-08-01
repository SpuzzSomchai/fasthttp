using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI.Server.Commands
{
    /// <summary>
    /// A command to edit the configuration file of the web server
    /// </summary>
    public class CommandEditCfg : ICommand
    {
        public CommandResult ExecuteCommand(string[] args)
        {
            try
            {
                Process.Start("notepad.exe", Environment.CurrentDirectory + "\\www\\cfg\\config.fcfg");
                return new CommandResult { ExitCode = 0, Message = "", State = 0 };
            }
            catch (Exception)
            {
                return new CommandResult { ExitCode = -1, Message = "", State = 0 };
            }
        }

        public void GetHelp(string locale)
        {
            
        }

        public string GetName()
        {
            return "editcfg";
        }
    }
}
