using System;
using System.Collections.Generic;
using System.IO;
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
            if(args.Length < 1)
            {
                Console.WriteLine("FastHTTPS Configuration Parsing Test Tool\nUsage: parsingtest <config_path>");
                return;
            }
            if(!File.Exists(args[0]))
            {
                Console.WriteLine("Error: Config does not exist!");
                return;
            }
            ServerConfigurationParser scp = new ServerConfigurationParser(args[0]);
            try
            {
                scp.Parse();
                scp.DisplayResultsTable();
            }
            catch (ConfigParsingException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
