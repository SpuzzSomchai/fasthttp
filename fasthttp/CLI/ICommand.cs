using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    public interface ICommand
    {
        string GetName();
        void GetHelp(string locale); //TODO implement locale functionality
        CommandResult ExecuteCommand(string[] args);
    }
}
