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
        public delegate void HttpCgiCallEventHandler(string cgiPath); //TODO enable CGI support. Implement method to check file extension to run a certain CGI executable. CGI executable use stdout for output results.

        /// <summary>
        /// An event that fires whenever an exception occurs
        /// </summary>
        public event HttpExceptionEventHandler ExceptionOccured;

        /// <summary>
        /// An event that fires when the server has started
        /// </summary>
        public event HttpGenericEventHandler ServerStarted;

        private ServerConfiguration config;
        private volatile Thread[] supportingThreads;
        private HttpListener hl;
        private Logger logger;
        private string htmlFolder;

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
            htmlFolder = config.WWWFolder + "\\html";
            if (!config.DisableLogging) logger = new StackedLogger(new ConsoleLogger(), new BufferedFileLogger(Path.Combine(config.WWWFolder, "logs\\latest.log")));
        }

        private async Task ServeFile(HttpListenerContext ctx, string path, int statusCode = 200)
        {
            logger.Log(LogLevel.INFORMATION, "Serve file " + path);
            //TODO check if file can be accessed and if it exists. Throw appropriate error codes if not found/not accessible
            //TODO improve and make buffered write, CopyToAsync could be slow
            ctx.Response.StatusCode = statusCode;
            ctx.Response.ContentType = "text/html"; //TODO determine mime type for file served
            using(FileStream fs = new FileStream(path, FileMode.Open))
                await fs.CopyToAsync(ctx.Response.OutputStream);
            ctx.Response.Close();
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
            {//TODO consider removing await keywords since file serving does not require itself to be executed sequentially
                try
                {
                    var context = await hl.GetContextAsync();
                    logger.Log(LogLevel.INFORMATION, "Got request from host {0} for URL {1}", context.Request.RemoteEndPoint, context.Request.RawUrl);
                    string pathInFS = Path.Combine(htmlFolder, context.Request.RawUrl.Substring(1).Replace('/', Path.DirectorySeparatorChar));
                    logger.Log("Debug path in file system relative to html dir: " + pathInFS);
                    //Check if specified path is a directory. If it is, check if any indexes exist and if not, display dir listing if its enabled

                    if (Directory.Exists(pathInFS))
                    {
                        bool foundIndexPage = false;
                        //TODO break out of loop to start dir listing
                        foreach(var indexPageName in config.IndexPages)
                        {
                            string indexPath = Path.Combine(pathInFS, indexPageName);
                            if(File.Exists(indexPath))
                            {
                                logger.Log(LogLevel.INFORMATION, "Not showing dir listing for URL, found index file " + indexPath);
                                await ServeFile(context, indexPath, 200);
                                foundIndexPage = true;
                                break;
                            }
                        }
                        if (foundIndexPage) continue;
                        //TODO show dir listing if enabled in config
                        logger.Log("Debug TODO show dir listing");
                    }
                    else await ServeFile(context, pathInFS);
                }
                catch (Exception ex)
                {
                    ExceptionOccured?.Invoke(ex);
                }
            }
        }
    }
}
