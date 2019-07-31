using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastHTTP.Server.Configuration.Parser;

namespace parsingtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            ServerConfigurationParser scp = new ServerConfigurationParser("config.txt");
            scp.Parse();
            scp.DisplayResultsTable();
        }
    }
}
