using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Logging
{
    /// <summary>
    /// A buffered file logger
    /// </summary>
    public class BufferedFileLogger : Logger
    {
        private string filePath;
        private FileStream stream;
        private int currentPos = 0;

        public BufferedFileLogger(string filePath, int bufSize = 4096)
        {
            this.filePath = filePath;
            stream = new FileStream(filePath, FileMode.Create);
        }

        private async void WriteString(string s)
        {
            byte[] str = Encoding.UTF8.GetBytes(s+"\n");
            await stream.WriteAsync(str, currentPos, str.Length);
        }

        public override void Log(LogLevel level, string message)
        {
            WriteString(String.Format(Prefix, DateTime.Now, level, message));
        }

        public override void Log(LogLevel level, string format, params object[] obj)
        {
            WriteString(String.Format(Prefix, DateTime.Now, level, String.Format(format, obj)));
        }

        public override void Log(string message)
        {
            WriteString(message);
        }

        public override void PerformCleanup()
        {
            stream.Flush();
            stream.Close();
        }
    }
}
