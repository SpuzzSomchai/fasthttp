using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.CLI
{
    /// <summary>
    /// A class to process arguments passed through the command line
    /// </summary>
    public class CliArgumentProcessor
    {
        public delegate void ArgProcessorErrorEventHandler(string optionName, string value, CliArgumentProcessorErrorType errorType);
        /// <summary>
        /// Fired whenever a processing error occurs
        /// </summary>
        public event ArgProcessorErrorEventHandler Error;

        /// <summary>
        /// Gets or sets whether the processor should catch and report unknown options (default is off)
        /// </summary>
        public bool CatchUnknownOptions { get; set; }

        private string[] arguments;
        private CliArgumentOption[] programOptions;
        private Dictionary<string, object> options;
        private List<string> remainder;
        private string command;

        /// <summary>
        /// Creates a new CLI argument processor with the specified arguments
        /// </summary>
        /// <param name="args">The array of arguments to process</param>
        public CliArgumentProcessor(string[] args, params CliArgumentOption[] programOptions)
        {
            arguments = args;
            options = new Dictionary<string, object>();
            remainder = new List<string>();
            CatchUnknownOptions = false;
            this.programOptions = programOptions;
        }

        /// <summary>
        /// Raises an error for the Error event
        /// </summary>
        /// <param name="a">Option Name</param>
        /// <param name="b">Value</param>
        /// <param name="c">Error type</param>
        public void RaiseError(string a, string b, CliArgumentProcessorErrorType c)
        {
            Error?.Invoke(a, b, c);
        }

        /// <summary>
        /// Checks if the specified option has been specified
        /// </summary>
        /// <param name="o">The option to locate</param>
        /// <returns>A boolean value with the result of the lookup</returns>
        public bool HasOption(string o)
        {
            return options.ContainsKey(o);
        }

        /// <summary>
        /// Retrieves the value of an option. Returns null when nothing has been found.
        /// </summary>
        /// <param name="o">The option to look for</param>
        /// <returns></returns>
        public object GetOptionValue(string o)
        {
            if (!HasOption(o)) return null;
            return options[o];
        }

        /// <summary>
        /// Begins the process of parsing all arguments
        /// </summary>
        public void ParseArguments()
        {
            if (arguments.Length < 1) return;
            command = arguments[0];
            if ((arguments.Length >= 1) && (arguments.Length < 2)) return;
            Queue<string> argQueue = new Queue<string>(arguments);
            List<string> lst = arguments.ToList();
            
            while(argQueue.Count > 0)
            {
                string argument = argQueue.Dequeue();
                if(argument.StartsWith(CliArgumentOption.CliOptionPrefix))
                {
                    string optionName = argument.Substring(CliArgumentOption.CliOptionPrefix.Length).Trim();
                    IEnumerable<CliArgumentOption> results = programOptions.Where(x => x.Name.ToLower() == optionName.ToLower());
                    if (results.Count() < 1)
                    {
                        if (!CatchUnknownOptions) continue;
                        Error?.Invoke(optionName, "", CliArgumentProcessorErrorType.UnknownOption);
                        continue;
                    }
                    CliArgumentOption opt = results.ElementAt(0);
                    
                    if(opt.ExpectedValueType != CliArgumentOptionValueType.None)
                    {
                        int valueIndex = lst.IndexOf(argument) + 1;
                        object val = lst.ElementAtOrDefault(valueIndex); //The value
                        if (argQueue.Count == 0) return;
                        argQueue.Dequeue(); //Remove next item (it is the value of the argument option)
                        if (val == null)
                        {
                            Error?.Invoke(optionName, "", CliArgumentProcessorErrorType.ValueExpected);
                            continue;
                        }
                        //Check if value is of the right type
                        switch(opt.ExpectedValueType)
                        {
                            case CliArgumentOptionValueType.CommaSeparatedList:
                                val = val.ToString().Split(',');
                                break;
                            case CliArgumentOptionValueType.Integer:
                                int t;
                                string strv = val.ToString();
                                if(!int.TryParse(strv, out t))
                                {
                                    Error?.Invoke(optionName, strv, CliArgumentProcessorErrorType.IntegerExpected);
                                    break;
                                }
                                val = t;
                                break;
                            case CliArgumentOptionValueType.String:
                                val = val.ToString();
                                break;
                        }
                        options[optionName] = val;
                        
                    } else //This is a regular option
                    {
                        options[optionName] = new CliArgumentValueEmpty();
                    }

                } else
                {
                    remainder.Add(argument);
                }
            }
            if(remainder.Count > 0) remainder.Remove(remainder[0]);
        }

#if DEBUG
        /// <summary>
        /// Debug function to map options to values. Does not have much practical use.
        /// </summary>
        public void DebugMap()
        {
            Console.WriteLine("CliArgumentProcessor.options:");
            foreach(var kv in options)
            {
                if(kv.Value.GetType().IsAssignableFrom(typeof(List<string>)))
                    Console.WriteLine("  {0} = [Comma List]", kv.Key);
                else Console.WriteLine("  {0} = {1}", kv.Key, kv.Value);
            }
            Console.WriteLine("\nCliArgumentProcessor.remainder");
            foreach (var r in remainder)
                Console.WriteLine("  " + r);
            Console.WriteLine("\nCommand: {0}", command);
        }
#endif
    }
}
