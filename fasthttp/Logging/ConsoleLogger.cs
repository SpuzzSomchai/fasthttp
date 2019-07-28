using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Logging
{
    /// <summary>
    /// A logger which logs to stdout
    /// </summary>
    public class ConsoleLogger : Logger
    {
        private ConsoleColor GetFgColor(LogLevel level)
        {
            if (level == LogLevel.ERROR) return ConsoleColor.Red;
            else if (level == LogLevel.WARNING) return ConsoleColor.Yellow;
            else return ConsoleColor.White;
        }

        public override void Log(LogLevel level, string message)
        {
            Console.ForegroundColor = GetFgColor(level);
            Console.WriteLine(Prefix, DateTime.Now, level, message);
            Console.ResetColor();
        }

        public override void Log(LogLevel level, string format, params object[] obj)
        {
            Console.ForegroundColor = GetFgColor(level);
            Console.WriteLine(Prefix, DateTime.Now, level, String.Format(format, obj));
            Console.ResetColor();
        }

        public override void Log(string message)
        {
            Console.WriteLine(message);
        }

        public override void PerformCleanup()
        {
            //Console does not need any kind of cleanup, its done automatically
        }
    }
}
