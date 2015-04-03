using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace csutils.FileFormats.INI
{
    /// <summary>
    /// represents a ini-file
    /// </summary>
    public class IniFile
    {
        private List<IniSection> sections;
        /// <summary>
        /// underlaying list of sections that store the data
        /// </summary>
        protected List<IniSection> IniSections
        {
            get { return sections; }
            set { sections = value; }
        }

        /// <summary>
        /// create a new, empty ini-file
        /// </summary>
        public IniFile()
        {
            IniSections = new List<IniSection>();
            IniSections.Add(new IniSection());
        }

        /// <summary>
        /// count of sections
        /// </summary>
        public int SectionCount
        {
            get { return IniSections.Count; }
        }

        /// <summary>
        /// gets a list of all section names, including the default-section ("")
        /// </summary>
        public IEnumerable<string> SectionNames
        {
            get
            {
                return IniSections.Select(s => s.Name);
            }
        }

        /// <summary>
        /// gets all sections
        /// </summary>
        public IniSection[] Sections
        {
            get
            {
                return IniSections.ToArray();
            }
        }

        /// <summary>
        /// gets a section 
        /// </summary>
        /// <param name="section"></param>
        /// <returns>get: returns null if section not found</returns>
        public IniSection this[string section]
        {
            get
            {
                return IniSections.FirstOrDefault(f=>f.Name==section);
            }
            set
            {
               
                for(int i=0; i<IniSections.Count; i++)
                    if(IniSections[i].Name == section)
                    {
                        IniSections[i] = value;
                        return;
                    }

                IniSections.Add(value);
            }
        }


        /// <summary>
        /// gets or sets a value of the given section. set: If the given section does not exist it will be created
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns>returns null when section or key not found</returns>
        public string this[string section, string key]
        {
            get
            {
                IniSection sec = IniSections.FirstOrDefault(f => f.Name == section);
                if (sec != null)
                    return sec[key];
                return null;
            }
            set
            {
                IniSection sec = IniSections.FirstOrDefault(f => f.Name == section);
                if (sec == null)
                {
                    sec = new IniSection() { Name = section };
                    IniSections.Add(sec);
                }
                sec[key] = value;
            }
        }

        /// <summary>
        /// creates a new object and loads the content from the stream. IniFileLoadStrategy.Merge is used
        /// </summary>
        /// <param name="s"></param>
        public IniFile(Stream s) : this()
        {
            string content;
            using(StreamReader sr = new StreamReader(s))
            {
                content = sr.ReadToEnd();
            }
            Load(content);
        }

        /// <summary>
        /// creates a new object and loads the content. IniFileLoadStrategy.Merge is used
        /// </summary>
        /// <param name="content"></param>
        public IniFile(string content) : this()
        {
            Load(content);
        }

        /// <summary>
        /// loads the content in ini-format to the object. IniFileLoadStrategy.Merge is used
        /// </summary>
        /// <param name="content"></param>
        public void Load(string content)
        {
            Load(content, IniFileLoadStrategy.Merge);
        }

        /// <summary>
        /// loads the content in ini-format to the object
        /// </summary>
        /// <param name="content"></param>
        /// <param name="strategy"></param>
        public void Load(string content, IniFileLoadStrategy strategy)
        {
            var data = IniParser.Parse(content);

            switch(strategy)
            {
                case IniFileLoadStrategy.Replace:
                    IniSections = data;
                    break;

                case IniFileLoadStrategy.Add:                    
                    foreach (IniSection s in data)
                        foreach (string key in s.Keys)
                            if (this[s.Name, key] == null)
                                this[s.Name, key] = s[key];
                    break;

                case IniFileLoadStrategy.Update:
                    foreach (IniSection s in data)
                        foreach (string key in s.Keys)
                            if (this[s.Name, key] != null)
                                this[s.Name, key] = s[key];
                    break;

                case IniFileLoadStrategy.Merge:
                    foreach (IniSection s in data)
                        foreach (string key in s.Keys)
                            this[s.Name, key] = s[key];
                    break;
            }
        }

        /// <summary>
        /// Save the ini file 
        /// </summary>
        /// <param name="file"></param>
        public void Save(string file)
        {
            File.WriteAllText(file, ToString());
        }

        /// <summary>
        /// converts into ini-format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(IniSection s in IniSections.OrderBy(o=>o.Name))
            {
                sb.Append(s.ToString());
                sb.Append("\r\n\r\n");
            }
            return sb.ToString();
        }
    }
}
