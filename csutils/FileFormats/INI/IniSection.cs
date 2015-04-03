using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace csutils.FileFormats.INI
{
    /// <summary>
    /// Section of an ini file
    /// </summary>
    public class IniSection
    {
        private string name;
        /// <summary>
        /// Name of the section. [ and ] will be cut away since they are part of the ini-syntax
        /// </summary>
        public string Name
        {
            get { return name; }
            set 
            {
                if (value == null)
                    return;

                string str = value.Trim();
                str = Regex.Replace(str, @"[\[\]]", "").Trim();

                name = str;
            }
        }


        /// <summary>
        /// names of the containing keys
        /// </summary>
        public string[] Keys
        {
            get
            {
                return Values.Keys.ToArray();
            }
        }

        /// <summary>
        /// compares to objects. Equality is given when the sectionname is equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(!(obj is IniSection))
                return false;

            return Name == ((IniSection)obj).Name;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        private Dictionary<string,string> values;
        /// <summary>
        /// the underlaying dictionary that holds the values
        /// </summary>
        protected Dictionary<string,string> Values
        {
            get { return values; }
            set { values = value; }
        }
        /// <summary>
        /// creates a new section-object. By default it has an empty name (-> it is the default-section)
        /// </summary>
        public IniSection()
        {
            Values = new Dictionary<string, string>();
            Name = "";
        }

        /// <summary>
        /// creates a new section-object
        /// </summary>
        /// <param name="name">name of the section, see  <see cref="IniSection.Name"/> </param>
        public IniSection(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// gets or sets a value. if value is null the key will be removed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (Values.ContainsKey(key))
                    return Values[key];
                return null;
            }
            set
            {
                if (value != null)
                    Values[key] = value;
                else
                    Values.Remove(key);
            }
        }

        /// <summary>
        /// count of the entries
        /// </summary>
        public int Count
        {
            get
            {
                return Values.Count;
            }
        }

        /// <summary>
        /// converts the object to string (valid ini-format)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Name != "")
            {
                sb.Append("[");
                sb.Append(Name);
                sb.Append("]");
                sb.Append("\r\n");
            }

            foreach(string key in Values.Keys)
            {
                sb.Append(key);
                sb.Append("=");
                sb.Append(Values[key]);
                sb.Append("\r\n");
            }

            return sb.ToString();
        }

    }

}
