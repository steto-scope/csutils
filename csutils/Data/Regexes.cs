using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace csutils.Data
{
    /// <summary>
    /// Class with predefined regular expressions
    /// </summary>
    public class Regexes
    {
        private static readonly Regex xmltag = new Regex(@"<\s*(\w+)\s*((""|').*?(""|')|[^"">]+)*?\s*>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
        private static readonly Regex invalidUriChars = new Regex(@"[^ \t-._~:/?#\[\]@!$&'()*\+,;=a-zA-Z0-9]+", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
        private static readonly Regex quotes = new Regex("[“”‘’\"']+", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
        private static readonly Regex nonasciichars = new Regex(@"[^\u0000-\u00FF]+", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline);
        

        /// <summary>
        /// Matches any xml/html tag. Does not care about open or closing tags. Groups[1] contains tag name
        /// </summary>
        public static Regex XMLTagRegex
        {
            get
            {
                return xmltag;
            }
        }

        /// <summary>
        /// Matches any invalid URI character according to RFC 3986, section 2 http://tools.ietf.org/html/rfc3986#section-2
        /// </summary>
        public static Regex InvalidUriCharacterRegex
        {
            get
            {
                return invalidUriChars;
            }
        }

        /// <summary>
        /// Matches any quotationmarks  (“, ”, ‘, ’, \", ')
        /// </summary>
        public static Regex QuotesRegex
        {
            get
            {
                return quotes;
            }
        }

        /// <summary>
        /// Matches any non-ASCII characters  (Unicode Block 0000 to 00FF)
        /// </summary>
        public static Regex NonASCIICharacterRegex
        {
            get
            {
                return nonasciichars;
            }
        }
    }
}
