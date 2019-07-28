using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Logging
{
    /// <summary>
    /// A logger consisting of multiple loggers
    /// </summary>
    public class StackedLogger : Logger
    {
        private List<Logger> loggers;

        public StackedLogger(params Logger[] loggers)
        {
            this.loggers = loggers.ToList();
        }

        public override void Log(LogLevel level, string message)
        {
            loggers.ForEach(x => x.Log(level, message));
        }

        public override void Log(LogLevel level, string format, params object[] obj)
        {
            loggers.ForEach(x => x.Log(level, format, obj));
        }

        public override void Log(string message)
        {
            loggers.ForEach(x => x.Log(message));
        }

        public override void PerformCleanup()
        {
            loggers.ForEach(x => x.PerformCleanup());
        }
    }
}
