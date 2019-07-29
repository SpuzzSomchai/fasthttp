using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.IO.Mime
{
    /// <summary>
    /// A parser to help parse mime type definition files
    /// </summary>
    public class MimeDefinitionsFileParser
    {
        /// <summary>
        /// The characters used to define a single line comment in a mime definition file
        /// </summary>
        public const string MDF_COMMENT = "#";

        private string filePath;

        /// <summary>
        /// Creates a new parser for the specified file name
        /// </summary>
        public MimeDefinitionsFileParser(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Parses the definition file and returns definitions as a dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Parse()
        {
            Dictionary<string, string> mimes = new Dictionary<string, string>();
            TextReader tr = new StreamReader(new FileStream(this.filePath, FileMode.Open));
            foreach(var line in tr.ReadToEnd().Split('\n'))
            {
                string formattedLine = line.Trim();
                if (formattedLine == "") continue;
                else if (formattedLine.StartsWith(MDF_COMMENT)) continue;
                else if (!formattedLine.Contains(" ")) continue;
                string[] pair = formattedLine.Split(' ');
                mimes[pair[0]] = pair[1];
            }
            return mimes;
        }
    }
}
