using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHTTP.IO.Mime
{
    /// <summary>
    /// A class representing a database consisting of mime type definitions
    /// </summary>
    public class MimetypeDatabase
    {
        /// <summary>
        /// The path specified for the mimetype database
        /// </summary>
        public string Path { get; protected set; }

        private Dictionary<string, string> mimes;

        /// <summary>
        /// Creates a new mime database object from the specified definition file
        /// </summary>
        public MimetypeDatabase(string path)
        {
            MimeDefinitionsFileParser parser = new MimeDefinitionsFileParser(path);
            mimes = parser.Parse();
        }

        /// <summary>
        /// Returns the mime type for the specified file name. Does not check magic numbers for performance reasons.
        /// </summary>
        /// <param name="fileName">The file path to return the mimetype from</param>
        /// <returns></returns>
        public string GetMimeType(string fileName)
        {
            var spf = fileName.Split('.');
            var ext = spf[spf.Length - 1];
            if (mimes.ContainsKey(ext))
                return mimes[ext];
            return "application/octet-stream";
        }
    }
}
