using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    public static class ConsoleExtensions
    {
        public static void WriteLineError(string text, params object[] x)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text, x);
            Console.ResetColor();
        }

        public static void Print(string text, params object[] x)
        {
            Console.WriteLine(text, x);
        }

        public static void PrintColor(ConsoleColor color, string text, params object[] x)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text, x);
            Console.ResetColor();
        }
    }
}
