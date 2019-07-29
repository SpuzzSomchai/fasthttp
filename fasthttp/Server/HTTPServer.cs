using FastHTTP.CLI;
using FastHTTP.IO.Mime;
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
        private MimetypeDatabase mimes;

        /// <summary>
        /// Creates a new HTTP/HTTPS server with the specified configuration
        /// </summary>
        /// <param name="cfg">The configuration to use</param>
        public HTTPServer(ServerConfiguration cfg)
        {//TODO add proper exception handling and do not assume everything is read write
            config = cfg;
            supportingThreads = new Thread[config.Threads];
            hl = new HttpListener();
            hl.Prefixes.Add("http://*:" + config.HttpPort + "/");
            if(config.EnableHTTPS) hl.Prefixes.Add("http://*:" + config.HttpsPort + "/");
            if (!Directory.Exists(config.WWWFolder)) Directory.CreateDirectory(config.WWWFolder);
            if (!Directory.Exists(config.WWWFolder + "\\logs")) Directory.CreateDirectory(config.WWWFolder + "\\logs");
            if (!Directory.Exists(config.WWWFolder + "\\html")) Directory.CreateDirectory(config.WWWFolder + "\\html");
            if (!Directory.Exists(config.WWWFolder + "\\cfg")) Directory.CreateDirectory(config.WWWFolder + "\\cfg");
            if (!File.Exists(config.WWWFolder + "\\cfg\\mimetypes.lst")) File.WriteAllText(config.WWWFolder + "\\cfg\\mimetypes.lst", "# Add your mimes here"); //TODO add default mime type list from embedded resources
            htmlFolder = config.WWWFolder + "\\html";
            if (!config.DisableLogging) logger = new StackedLogger(new ConsoleLogger(), new BufferedFileLogger(Path.Combine(config.WWWFolder, "logs\\latest.log")));
            mimes = new MimetypeDatabase(config.WWWFolder + "\\cfg\\mimetypes.lst");
        }

        private async Task ServeFile(HttpListenerContext ctx, string path, int statusCode = 200)
        {
            logger.Log(LogLevel.INFORMATION, "Serve file " + path);
            //TODO check if file can be accessed and if it exists. Throw appropriate error codes if not found/not accessible
            //TODO improve and make buffered write, CopyToAsync could be slow
            ctx.Response.StatusCode = statusCode;
            ctx.Response.ContentType = mimes.GetMimeType(path); //TODO determine mime type for file served
            using(FileStream fs = new FileStream(path, FileMode.Open))
                await fs.CopyToAsync(ctx.Response.OutputStream);
            ctx.Response.Close();
        }

        private async Task ServeHtml(HttpListenerContext ctx, string htmlData)
        {
            byte[] htmlDataBytes = Encoding.UTF8.GetBytes(htmlData);
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html";
            ctx.Response.ContentLength64 = htmlDataBytes.Length;
            ctx.Response.ContentEncoding = Encoding.UTF8;
            await ctx.Response.OutputStream.WriteAsync(htmlDataBytes, 0, htmlDataBytes.Length);
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
                    var context = await hl.GetContextAsync(); //TODO clone request maybe? error An operation was attempted on a nonexistent network connection is thrown when too many reloads are occuring pls fix
                    logger.Log(LogLevel.INFORMATION, "Got request from host {0} for URL {1}", context.Request.RemoteEndPoint, context.Request.RawUrl);
                    //TODO check for malformed paths
                    //TODO fix url system
                    string pathInFS = Path.Combine(htmlFolder, Uri.UnescapeDataString(context.Request.Url.AbsolutePath.Substring(1)).Replace('/', Path.DirectorySeparatorChar));
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
                                ServeFile(context, indexPath, 200);
                                foundIndexPage = true;
                                break;
                            }
                        }
                        if (foundIndexPage) continue;
                        //TODO show dir listing if enabled in config
                        //TODO allow user to customize the style of the dir listing like apache web server
                        //TODO check if cookies specify listing format
                        #region Simple file listing - pls remove when better implementation is added
                        var html = @"<meta charset='utf-8'><style>a { text-decoration: none; }</style><h2>Directory listing of " + Uri.UnescapeDataString(context.Request.RawUrl) + "</h2><hr>";
                        if (context.Request.RawUrl != "/") html += "<a href=\"..\">🔼&nbsp;&nbsp;&nbsp;Up..</a><br>";
                        foreach(var d in Directory.GetDirectories(pathInFS))
                        {
                            if(context.Request.RawUrl != "/") html += string.Format("<a href=\"{1}/{0}\">📁 {0}</a><br>", Path.GetFileName(d), context.Request.Url.AbsolutePath);
                            else html += string.Format("<a href=\"/{0}\">📁 {0}</a><br>", Path.GetFileName(d));
                        }
                        foreach(var f in Directory.GetFiles(pathInFS))
                        {
                            if (context.Request.RawUrl != "/") html += string.Format("<a href=\"{1}/{0}\">📄 {0}</a><br>", Path.GetFileName(f), context.Request.Url.AbsolutePath);
                            else html += string.Format("<a href=\"/{0}\">📄 {0}</a><br>", Path.GetFileName(f));
                        }
                        ServeHtml(context, html);
                        #endregion
                    }
                    else ServeFile(context, pathInFS);
                }
                catch (Exception ex)
                {
                    ExceptionOccured?.Invoke(ex);
                }
            }
        }
    }
}
