using csutils.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#if CLR
using System.Windows.Media;
using csutils.Globalisation;
#endif 

namespace System
{
    /// <summary>
    /// Class for strings extensions
    /// </summary>
    public static class csutilsStringExtension
    {
#if CLR
        /// <summary>
        /// Convert hexadecimal to color object
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color? ToColor(this string hex)
        {
            hex = hex.Trim();
            int blue = 0;
            int red = 0;
            int green = 0;

            if (hex == null)
                return null;

            if (System.Text.RegularExpressions.Regex.IsMatch(hex, @"^[#]?([0-9]|[a-f]|[A-F]){6}$"))
            {
                int hashtag = hex.StartsWith("#") ? 0 : 1;

                red = int.Parse(hex.Substring(1 - hashtag, 2), NumberStyles.HexNumber);
                green = int.Parse(hex.Substring(3 - hashtag, 2 ), NumberStyles.HexNumber);
                blue = int.Parse(hex.Substring(5 - hashtag, 2 ), NumberStyles.HexNumber);
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(hex, @"^[#]?([0-9]|[a-f]|[A-F]){3}$"))
            {
                int hashtag = hex.StartsWith("#") ? 0 : 1;

                red = int.Parse(hex.Substring(1-hashtag, 1), NumberStyles.HexNumber);
                green = int.Parse(hex.Substring(2 - hashtag, 1), NumberStyles.HexNumber);
                blue = int.Parse(hex.Substring(3 - hashtag, 1), NumberStyles.HexNumber);
                red += 16 * red;
                green += 16 * green;
                blue += 16 * blue;
            }
            else
                return null;
            

            return Color.FromRgb((byte)red, (byte)green, (byte)blue);
        }
#endif

        /// <summary>
        /// Convert to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? ToBool(this string value)
        {
            if (value != null)
            {
                if (value.Trim().ToLower() == "true")
                    return true;
                if (value.Trim().ToLower() == "false")
                    return false;
            }
            return null;
        }

        /// <summary>
        /// Convert to bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string value, bool defaultValue)
        {
            var b = value.ToBool();
            if (b == null)
                return defaultValue;
            return b.Value;
        }

        /// <summary>
        /// Convert to int
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Int-Wert oder null bei fehler</returns>
        public static int? ToInt(this string value)
        {
            int o = 0;
            if (Int32.TryParse(value, out o))
                return o;
            return null;
        }

        /// <summary>
        /// Convert to int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string value, int defaultValue)
        {
            var i = value.ToInt();
            if (i == null)
                return defaultValue;
            return i.Value;
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Int-Wert oder null bei fehler</returns>
        public static double? ToDouble(this string value)
        {
            double o = 0;
            if (double.TryParse(value, out o))
                return o;
            return null;
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultValue)
        {
            var d = value.ToDouble();
            if (d == null)
                return defaultValue;
            return d.Value;
        }

        /// <summary>
        /// Increases string by given value if it starts with a number (for example "300" -> "301" or "300px"-> "301px")
        /// </summary>
        /// <param name="value"></param>
        /// <param name="number"></param>
        /// <returns>if numeric, the new increased value, else the original string</returns>
        public static string TryAdd(this string value, double number)
        {
            Match m = Regex.Match(value, @"^\-?\d+(\.\d+)?", RegexOptions.Compiled);
            if (m != null && m.Success)
            {
                string num = value.Substring(0, m.Length);
                string rest = value.Substring(m.Length, value.Length - m.Length);
                num = (double.Parse(num) + number).ToString();
                return num + rest;
            }
            return value;
        }

        
        /// <summary>
        /// Gets the index of a string. Returns -1 if not found
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf(this string[] ar, string value)
        {
            for (int i = 0; i < ar.Length; i++)
            {
                if (ar[i] == value)
                    return i;
            }
            return -1;
        }


        /// <summary>
        /// Finds a substring by a regex and replaces it with given character
        /// </summary>
        /// <param name="value"></param>
        /// <param name="regex">regular expression for searching</param>
        /// <param name="chr">Character to replace</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string FillReplace(this string value, string regex, char chr)
        {
            foreach (Match m in Regex.Matches(value, regex))
            {
                string temp = "";
                temp = temp.PadLeft(m.Length, chr);
                value = value.Substring(0, m.Index) + temp + value.Substring(m.Index + temp.Length, value.Length - m.Index - temp.Length);
            }
            return value;
        }

        /// <summary>
        /// Trim quotes (both " and ')
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimQuotes(this string str)
        {
            while (str.StartsWith("\"") || str.StartsWith("'"))
                str = str.Substring(1);
            while (str.EndsWith("\"") || str.EndsWith("'"))
                str = str.Substring(0, str.Length - 1);
            return str;
        }


        /// <summary>
        /// Removes extension
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveExtension(this string str)
        {
            int i = str.LastIndexOf(".");
            if (i > 0)
                return str.Substring(0, i);
            return str;
        }
        /// <summary>
        /// Determines the first occourance of any of the given strings
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static int IndexOfAny(this string str, string[] args)
        {
            if (args == null)
                throw new ArgumentNullException();

            int lowest = str.Length;
            int temp = 0;
            foreach (string s in args)
            {
                if (s == null)
                    continue;

                temp = str.IndexOf(s);
                if (temp >= 0 && temp < lowest)
                    lowest = temp;
            }
            if (lowest == str.Length)
                lowest = -1;
            return lowest;
        }

     

        /// <summary>
        /// returns true if the string ends with any of the given strings
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static bool EndsWith(this string str, IEnumerable<string> strings)
        {
            if (strings == null)
                return false;
            return strings.Where(w=>w!=null).Any(a => str.EndsWith(a));
        }

        
        /// <summary>
        /// Extracts the Filename of a FileInfo-Object without Extension
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static string FileName(this FileInfo fi)
        {
            return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
        }
       

        /// <summary>
        /// Returns the given path with a terminating \
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathWithSlash(this string path)
        {
            if (path.Length < 1)
                return path;

            if (!path.EndsWith("\\"))
                return path + "\\";
            return path;
        }

        /// <summary>
        /// Removes therminating backslashes of a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathWithoutSlash(this string path)
        {
            while (path.EndsWith("\\"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        /// <summary>
        /// Changes the directoryname
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <returns></returns>
        public static DirectoryInfo ChangeName(this DirectoryInfo info, string name)
        {
            if (name == null)
                return info;
            if (string.IsNullOrEmpty(name.Trim()))
                throw new ArgumentException("name can not be empty or null");
            if (Regex.IsMatch(name, @"[<>:/\""\\|?*]+"))
                throw new ArgumentException("Invalid Characters in name");

            if (info.Parent == null)
                return info;

            return new DirectoryInfo(info.Parent.FullName +  name);

        }

#if CLR
        /// <summary>
        /// Opens the directory / selects the file in windows explorer
        /// </summary>
        /// <param name="obj"></param>
        public static void OpenInWindowsExplorer(this FileSystemInfo obj)
        {
            OpenInWindowsExplorer(obj.FullName);
        }

        /// <summary>
        /// Opens the directory / selects the file in windows explorer
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static void OpenInWindowsExplorer(this string obj)
        {
            FileAttributes attr = File.GetAttributes(obj);
            string args = ((attr & FileAttributes.Directory) == FileAttributes.Directory ? "/root, " : "/select, ") + obj;
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("SystemRoot") + "\\explorer.exe", args);
            p.Start();
        }
#endif

        /// <summary>
        /// Checks if the string starts with a number
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StartsWithNumeric(this string str)
        {
            return Regex.IsMatch(str.Trim(), @"^\-?[0-9]{1,1}");
        }

        /// <summary>
        /// Converts a string into a FileSystemInfo object. If the string looks like a file (has an extension) a FileInfo will be created, otherwise a DirectoryInfo.
        /// If checkIfExists is true only existing files/directories are valid. If not, result is null.
        /// Environment variables will be recognized and replaced by their value.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checkIfExists"></param>
        /// <returns></returns>
        public static FileSystemInfo ToPath(this string path, bool checkIfExists = false)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            if (!checkIfExists)
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Extension != null && fi.Extension.Length > 0)
                    return fi;
                return new DirectoryInfo(path);
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                    return fi;
                else
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    return di.Exists ? di : null;
                }
            }
        }

        /// <summary>
        /// Converts a path into a FileInfo. Returns null if no conversion is possbile. Environment variables will be replace by their value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checkIfExists"></param>
        /// <returns></returns>
        public static FileInfo ToFileInfo(this string path, bool checkIfExists = false)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            FileInfo fi = new FileInfo(path);

            return (checkIfExists && fi.Exists) || !checkIfExists ? fi : null;
        }

        /// <summary>
        /// Converts a path into a DirectoryInfo. Returns null if no conversion is possbile. Environment variables will be replace by their value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checkIfExists"></param>
        /// <returns></returns>
        public static DirectoryInfo ToDirectoryInfo(this string path, bool checkIfExists = false)
        {
            path = Environment.ExpandEnvironmentVariables(path);
            DirectoryInfo di = new DirectoryInfo(path);

            return (checkIfExists && di.Exists) || !checkIfExists ? di : null;
        }

        /// <summary>
        /// Removes characters from a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type">Type of character escape</param>
        /// <param name="fillCharacter">if true the affected characters will be replaced with this char (keeps original string length)</param>
        /// <returns></returns>
        public static string Remove(this string str, CharacterType type, char? fillCharacter = null)
        {
            return str.Remove(type, ContentType.None, fillCharacter);
        }

        /// <summary>
        /// Removes characters from a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type">Type of character trim</param>
        /// <param name="contenttype">Type of content escape</param>
        /// <param name="fillCharacter">if true the affected characters will be replaced with this char (keeps original string length)</param>
        /// <returns></returns>
        public static string Remove(this string str, CharacterType type, ContentType contenttype, char? fillCharacter = null)
        {
            //escape content
            if((contenttype & ContentType.XMLTags) == ContentType.XMLTags)
                str = str.Remove(Regexes.XMLTagRegex, fillCharacter);
                   

            //escape characters
            if ((type & CharacterType.InvalidPathCharacters) == CharacterType.InvalidPathCharacters)
                str = str.Remove(new Regex("[" + new string(Path.GetInvalidPathChars()) + "?]+"), fillCharacter);
            if ((type & CharacterType.InvalidURLCharacters) == CharacterType.InvalidURLCharacters)
                str = str.Remove(Regexes.InvalidUriCharacterRegex, fillCharacter);
            if ((type & CharacterType.Whitespaces) == CharacterType.Whitespaces)
                str = str.Remove(new Regex(@"\s+"), fillCharacter);
            if ((type & CharacterType.Quotations) == CharacterType.Quotations)
                str = str.Remove(Regexes.QuotesRegex, fillCharacter);
            if ((type & CharacterType.NonASCIICharacters) == CharacterType.NonASCIICharacters)
                str = str.Remove(Regexes.NonASCIICharacterRegex, fillCharacter);
            return str;
        }

        /// <summary>
        /// Removes characters from a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="characters">string containing all characters to trim</param>
        /// <param name="fillCharacter">if true the affected characters will be replaced with this char (keeps original string length)</param>
        /// <returns></returns>
        public static string Remove(this string str, string characters, char? fillCharacter = null)
        {
            return str.Remove(new Regex("[" + characters + "]+", RegexOptions.Multiline | RegexOptions.Singleline), fillCharacter);
        }

        /// <summary>
        /// Removes characters from a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regex">Regex to match the trim characters</param>
        /// <param name="fillCharacter">if not null the affected characters will be replaced with this char (keeps original string length)</param>
        /// <returns></returns>
        public static string Remove(this string str, Regex regex, char? fillCharacter = null)
        {
            if (regex == null)
                throw new ArgumentException("regex can not be null");
            
            if (fillCharacter != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach(Match m in regex.Matches(str))
                {
                    sb.Append(str.Substring(sb.Length, m.Index - sb.Length));
                    sb.Append("".PadRight(m.Length,fillCharacter.Value));
                }
                sb.Append(str.Substring(sb.Length, str.Length - sb.Length));

                return sb.ToString();
            }
            else
            {
                return regex.Replace(str, "");
            }
        }

        /// <summary>
        /// Converts a string into a stream. Uses the specified Encoding to write the characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Stream ToStream(this string text, Encoding encoding)
        {
            if (encoding == null)
                throw new ArgumentException("encoding can not be null. Choose a valid constant");

            MemoryStream stream = new MemoryStream();
            byte[] buffer = encoding.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Converts a string into a stream. Uses Unicode encoding
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Stream ToStream(this string text)
        {
            return text.ToStream(Encoding.Unicode);
        }

        /// <summary>
        /// Translates a string into another language; Wrapper for TranslationManager.Translate()
        /// </summary>
        /// <param name="key"></param>
        /// <param name="channel"></param>
        /// <param name="targetCulture"></param>
        /// <returns></returns>
        public static string Translate(this string key, string channel = "", CultureInfo targetCulture = null)
        {
            return TranslationManager.Translate(key, channel, targetCulture);
        }


		/// <summary>
		/// Applies a regular expression to an array of strings. The regex gets evaluated against each element and every match stays in the array
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="regexPattern"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static string[] Filter(this string[] strings, string regexPattern, RegexOptions options = RegexOptions.None)
		{
			Regex r = new Regex(regexPattern, options);
			List<string> output = new List<string>();
			foreach(string str in strings)
				if (r.IsMatch(str))
					output.Add(str);
			return output.ToArray();
		}

		/// <summary>
		/// Applies a regular expression to a enumeration. The regex gets evaluated against each element and every match stays
		/// </summary>
		/// <param name="strings"></param>
		/// <param name="regexPattern"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static IEnumerable<string> Filter(this IEnumerable<string> strings, string regexPattern, RegexOptions options = RegexOptions.None)
		{
			Regex r = new Regex(regexPattern, options);
			foreach (string str in strings)
				if (r.IsMatch(str))
					yield return str;
		}
    }

    /// <summary>
    /// Specifies the characters 
    /// </summary>
    public enum CharacterType : int
    {
        /// <summary>
        /// escape all characters which are invalid for URL addresses
        /// </summary>
        InvalidURLCharacters = 1,
        /// <summary>
        /// escape all characters which are invalid for local paths
        /// </summary>
        InvalidPathCharacters = 2,
        /// <summary>
        /// escape all types of whitespaces 
        /// </summary>
        Whitespaces = 4,
        /// <summary>
        /// no quotations
        /// </summary>
        Quotations = 8,
        /// <summary>
        /// no HTML script tags
        /// </summary>
        NonASCIICharacters = 16,
    }

    /// <summary>
    /// Specifies the content to trim
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// escape all XML / HTML tags
        /// </summary>
        XMLTags = 2,
        /// <summary>
        /// escape nothing
        /// </summary>
        None = 1
    }
}
