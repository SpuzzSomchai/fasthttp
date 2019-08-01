using FastHTTP.HTTP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.CGI
{
    /// <summary>
    /// A client for the common gateway interface (CGI version 1.1).
    /// </summary>
    // TODO implement CGI spec 1.1 http://graphcomp.com/info/specs/cgi11.html
    public class CGIClient
    {
        /// <summary>
        /// The command used for the CGI client
        /// </summary>
        public string Command { get; set; }

        private int maxTimeout;

        /// <summary>
        /// Creates a new CGI 1.1 client using the specified binary
        /// </summary>
        /// <param name="binaryPath">The binary to use</param>
        /// <param name="maxTimeout">Maximum running time for php</param>
        public CGIClient(string binaryPath, int maxTimeout = 10000)
        {
            Command = binaryPath;
            this.maxTimeout = maxTimeout;
        }

        /// <summary>
        /// Executes the CGI executable with the specified arguments
        /// </summary>
        /// <param name="fileName">The file to execute</param>
        /// <param name="queryString">The query string to pass</param>
        /// <param name="method">The HTTP method to use</param>
        /// <param name="contentData">Additional data</param>
        /// <returns></returns>
        public CGIResult Execute(string fileName, string queryString, HttpMethod method, string contentData = "")
        {
            string result = "";
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo(Command);
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.Environment["REDIRECT_STATUS"] = "true";
            psi.Environment["GATEWAY_INTERFACE"] = "CGI/1.1";
            psi.Environment["SCRIPT_FILENAME"] = fileName;
            psi.Environment["SERVER_SOFTWARE"] = "fasthttps/1.0";
            switch(method)
            {//TODO implement post and other methods
                case HttpMethod.GET:
                    psi.Environment["REQUEST_METHOD"] = "GET";
                    break;
            }
            psi.Environment["QUERY_STRING"] = queryString;
            psi.Environment["CONTENT_LENGTH"] = "0";
            p.StartInfo = psi;
            p.Start();
            int bytesRead = 0;
            char[] buffer = new char[1024]; //1K buffer
            StringBuilder stdout = new StringBuilder();
            while((bytesRead = p.StandardOutput.Read(buffer, 0, buffer.Length)) != 0)
            {
                if(bytesRead != buffer.Length)
                {
                    for (int i = 0; i < bytesRead; i++)
                        stdout.Append(buffer[i]);
                }
                else stdout.Append(buffer);
            }
            p.WaitForExit(maxTimeout);
            var document = stdout.ToString();
            var documentLines = document.Split('\n');
            bool headersFound = false;
            var headers = "";
            for(int i=0;i<documentLines.Length;i++)
            {
                string line = documentLines[i];
                if(!headersFound)
                {
                    if (line.Trim() == "") headersFound = true;
                    headers += line + "\n";
                    continue;
                }
                else
                {
                    result += line + "\n";
                }
            }
            p.Dispose();
            //TODO [Completed] extract headers and feed them to web server
            return new CGIResult { Headers = HeadersParser.Parse(headers), OutDocument = result };
        }
    }
}
