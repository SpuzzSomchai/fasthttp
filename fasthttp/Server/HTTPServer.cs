using FastHTTP.CLI;
using FastHTTP.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastHTTP.Server
{
    /// <summary>
    /// The main class for the HTTP/HTTPS server
    /// </summary>
    public class HTTPServer
    {
        public delegate void HttpExceptionEventHandler(Exception e);
        public delegate void HttpGenericEventHandler();

        /// <summary>
        /// An event that fires whenever an exception occurs
        /// </summary>
        public event HttpExceptionEventHandler ExceptionOccured;

        /// <summary>
        /// An event that fires when the server has started
        /// </summary>
        public event HttpGenericEventHandler ServerStarted;

        private ServerConfiguration config;
        private Thread[] supportingThreads;
        private HttpListener hl;
        private Logger logger;

        /// <summary>
        /// Creates a new HTTP/HTTPS server with the specified configuration
        /// </summary>
        /// <param name="cfg">The configuration to use</param>
        public HTTPServer(ServerConfiguration cfg)
        {
            config = cfg;
            supportingThreads = new Thread[config.Threads];
            hl = new HttpListener();
            hl.Prefixes.Add("http://*:" + config.HttpPort + "/");
            if(config.EnableHTTPS) hl.Prefixes.Add("http://*:" + config.HttpsPort + "/");
            if (!Directory.Exists(config.WWWFolder)) Directory.CreateDirectory(config.WWWFolder);
            if (!Directory.Exists(config.WWWFolder + "\\logs")) Directory.CreateDirectory(config.WWWFolder + "\\logs");
            if (!Directory.Exists(config.WWWFolder + "\\html")) Directory.CreateDirectory(config.WWWFolder + "\\html");
            if (!config.DisableLogging) logger = new StackedLogger(new ConsoleLogger(), new BufferedFileLogger(Path.Combine(config.WWWFolder, "logs\\latest.log")));
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public async Task Start()
        {
            try
            {
                hl.Start();
                ServerStarted?.Invoke();
            }
            catch (HttpListenerException ex)
            {
                ExceptionOccured?.Invoke(ex);
            }
            while(true)
            {
                try
                {
                    var context = await hl.GetContextAsync();
                    logger.Log(LogLevel.INFORMATION, "Got request from host {0}", context.Request.RemoteEndPoint);
                    
                }
                catch (Exception ex)
                {
                    ExceptionOccured?.Invoke(ex);
                }
            }
        }
    }
}
