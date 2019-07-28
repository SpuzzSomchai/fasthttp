using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Logging
{
    /// <summary>
    /// The base class used to define a Logger
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// The prefix to use when logging an event (does not work for Log(string))
        /// </summary>
        public string Prefix { get; set; } = "[{0} {1}]: {2}";

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="level">The logging level to report</param>
        /// <param name="message">The message to log</param>
        public abstract void Log(LogLevel level, string message);

        /// <summary>
        /// Logs a formatted message with objects
        /// </summary>
        /// <param name="level">The logging level to report</param>
        /// <param name="format">The message format to use</param>
        /// <param name="obj">The objects/strings to log</param>
        public abstract void Log(LogLevel level, string format, params object[] obj);

        /// <summary>
        /// Logs a non-prefixed message
        /// </summary>
        /// <param name="message">The message to log</param>
        public abstract void Log(string message);

        /// <summary>
        /// Cleans up buffered resources
        /// </summary>
        public abstract void PerformCleanup();
    }
}
