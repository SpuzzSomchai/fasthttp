using FastHTTP.Server;
using FastHTTP.Server.REST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static FastHTTP.CLI.ConsoleExtensions;
using static System.ConsoleColor;

namespace FastHTTP.CLI.Server.Commands
{
    /// <summary>
    /// The CLI command used to start up the web server
    /// </summary>
    public class CommandStart : ICommand
    {
        private HTTPServer server;
        public CommandResult ExecuteCommand(string[] args)
        {
            CliArgumentProcessor argumentProcessor = new CliArgumentProcessor(args, new CliArgumentOption() { Name = "port", ExpectedValueType = CliArgumentOptionValueType.Integer }, 
                new CliArgumentOption { Name = "help", ExpectedValueType = CliArgumentOptionValueType.None },
                new CliArgumentOption { Name = "port-https", ExpectedValueType = CliArgumentOptionValueType.Integer },
                new CliArgumentOption { Name = "disable-log", ExpectedValueType = CliArgumentOptionValueType.None },
                new CliArgumentOption { Name = "www-dir", ExpectedValueType = CliArgumentOptionValueType.String },
                new CliArgumentOption { Name = "disable-dir-listing", ExpectedValueType = CliArgumentOptionValueType.None });
            argumentProcessor.CatchUnknownOptions = true;
            argumentProcessor.Error += FastHTTPServer.ArgumentProcessor_Error;
            argumentProcessor.ParseArguments();
            if(argumentProcessor.HasOption("help"))
            {
                GetHelp("default");
                return new CommandResult { ExitCode = 0, Message = "", State = null };
            }

            Console.WriteLine(@"                                             
 (                  )    )     )    )        
 )\ )     )      ( /( ( /(  ( /( ( /(        
(()/(  ( /(  (   )\()))\()) )\()))\())`  )   
 /(_)) )(_)) )\ (_))/((_)\ (_))/(_))/ /(/(   
(_) _|((_)_ ((_)| |_ | |(_)| |_ | |_ ((_)_\  
 |  _|/ _` |(_-<|  _|| ' \ |  _||  _|| '_ \) 
 |_|  \__,_|/__/ \__||_||_| \__| \__|| .__/  
                                     |_|     

FASTHTTP High Performance HTTP/HTTPS Web Server version 1.0
Copyright (C) Ralph Vreman 2019. All rights reserved.
");
            
            //Start the HTTP(s) server
            //This is the default configuration
            //TODO implement more configuration options soon

            server = new HTTPServer(new ServerConfiguration() {
                DisableLogging = argumentProcessor.HasOption("disable-log") ? (bool)argumentProcessor.GetOptionValue("disable-log") : false,
                DumpRequests = argumentProcessor.HasOption("dump-reqs") ? (bool)argumentProcessor.GetOptionValue("dump-reqs") : false,
                DumpRequestURLs = argumentProcessor.HasOption("dump-req-urls") ? (bool)argumentProcessor.GetOptionValue("dump-req-urls") : false,
                EnableHTTPS = argumentProcessor.HasOption("enable-https"),
                WWWFolder = argumentProcessor.HasOption("www-dir") ? (string)argumentProcessor.GetOptionValue("www-dir") : Path.Combine(Environment.CurrentDirectory, "www"),
                HttpPort = argumentProcessor.HasOption("port") ? (int)argumentProcessor.GetOptionValue("port") : 80,
                HttpsPort = argumentProcessor.HasOption("port-https") ? (int)argumentProcessor.GetOptionValue("port-https") : 443,
                SinglePageMode = argumentProcessor.HasOption("single-page"),
                SinglePagePath = argumentProcessor.HasOption("single-page") ? (string)argumentProcessor.GetOptionValue("single-page") : "",
                Threads = argumentProcessor.HasOption("threads") ? (int)argumentProcessor.GetOptionValue("threads") : 1,
                DisableDirListing = argumentProcessor.HasOption("disable-dir-listing")
            });
            server.ExceptionOccured += Server_OnException;
            server.ServerStarted += Server_ServerStarted;
#if DEBUG
            server.RegisterCGIClient(".php", "C:\\Users\\Ralph\\source\\repos\\fasthttp\\fasthttps\\bin\\Debug\\www\\bin\\php7\\php-cgi.exe");
            server.RegisterIndexPage("index.php");
            server.RegisterCGIClient(".bat", "cmdcgi.bat");
            server.RegisterIndexPage("index.bat");
            server.RegisterRESTApi(new RestAPITest());
#endif
            server.Start();
            Console.CancelKeyPress += Console_CancelKeyPress;
            
            while (server.IsRunning) ;
            return new CommandResult { ExitCode = 0, Message = "", State = null };
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            server.Stop();
            Console.WriteLine("Goodbye.");
            Environment.Exit(0);
        }

        private void Server_ServerStarted()
        {
            PrintColor(ConsoleColor.Cyan, "HTTP server has started.");
        }
        
        private void Server_OnException(Exception e)
        {
            /*if(typeof(HttpListenerException).IsAssignableFrom(e.GetType()))
            {
                WriteLineError("Error whilst starting server: {0}", e.Message);
                Environment.Exit(1);
            }*/
#if DEBUG
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Source);
            Console.WriteLine(e.StackTrace);
#endif
        }

        public void GetHelp(string locale)
        {
            Console.WriteLine("Usage: fasthttps start [flags]");
            Console.WriteLine(@"Flags:
  --disable-dir-listing      - Disables directory listing entirely
  --disable-log              - Disables logging (does not affect --dump-req-urls and --dump-reqs flag)
  --port <int 1-65535>       - Sets a custom port for the HTTP server
  --port-https <int 1-65535> - Sets a custom port for the HTTPS server
  --www-dir <path>           - Sets the working directory for the web server (defaults to [current dir]/www)");
        }

        public string GetName()
        {
            return "start";
        }
    }
}
