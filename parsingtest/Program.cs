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
            ServerConfigurationParser scp = new ServerConfigurationParser("config.txt");
            scp.Parse();
            Console.WriteLine("[Defined constants]\n");
            scp.DefinedConstants.Keys.All((s) =>
            {
                Console.WriteLine("  Constant Name: {0}, Constant Value: {1}", s, scp.DefinedConstants[s].Value);
                return true;
            });
            Console.WriteLine("\n[Defined Sections]\n");
            scp.DefinedSections.Keys.All((s) =>
            {
                Console.WriteLine("  Section Name: {0}", s);
                return true;
            });
        }
    }
}
