using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.Server.IPC
{
    /// <summary>
    /// A named pipe IPC server to communicate with FastHTTP through API
    /// </summary>
    public class IPCServer
    {
        private NamedPipeServerStream pipeServer;
        private string pipeName = "fasthttp\\server";
        private ServerExecutionContext sec;

        public bool Running { get; set; } = false;

        public IPCServer(ServerExecutionContext ctx, string pipeSuffix = "")
        {
            sec = ctx;
            pipeName += pipeSuffix;
        }

        public void Start()
        {
            Running = true;
            pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut);
            sec.ServerLog.Log(Logging.LogLevel.INFORMATION, "IPC server running at pipe " + pipeName);
            while (Running)
            {
                try
                {
                    pipeServer.WaitForConnection();
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024]; //Reads 8k messages
                    List<byte> constructedData = new List<byte>();
                    while((bytesRead = pipeServer.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        if (bytesRead != buffer.Length)
                        {
                            for (int i = 0; i < bytesRead; i++)
                                constructedData.Add(buffer[i]);
                        }
                        else constructedData.AddRange(buffer);
                    }
                    string receivedData = Encoding.UTF8.GetString(constructedData.ToArray()).Trim();
                    foreach(var ln in receivedData.Split('\n'))
                    {
                        string request = "null";
                        try
                        {
                            var line = ln.Trim();
                            if (line == "") continue;
                            string[] cmd = line.Split(' ');
                            request = cmd[0];
                            sec.ServerLog.Log(Logging.LogLevel.INFORMATION, "Received IPC Request {0}", request);
                            switch (cmd[0])
                            {
                                case "STOP-SERVER":
                                    sec.Server.Stop();
                                    break;
                                case "REGISTER-CGI-APPLICATION":
                                    string[] s = line.Split('"');
                                    string cgiExtension = s[1].Trim();
                                    string cgiBinPath = s[3].Trim();
                                    sec.Server.RegisterCGIClient(cgiExtension, cgiBinPath);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            sec.ServerLog.Log(Logging.LogLevel.ERROR, "Error completing IPC request \"{0}\". Message: {1}", request, ex.Message);
                            continue;
                        }
                        
                    }
                    pipeServer.Disconnect();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
