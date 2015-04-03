using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace csutils.FileFormats.INI
{
    /// <summary>
    /// Parser for Ini-Format
    /// </summary>
    public class IniParser
    {
        /// <summary>
        /// Parses a string into a list of Ini-Sections
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<IniSection> Parse(string content)
        {
            if (content == null)
                return null;

            StringReader sr = new StringReader(content);

            string line; //run variable
            List<IniSection> sections = new List<IniSection>(); //all sections
            IniSection currentSection = new IniSection(); //pointer of DEA
            sections.Add(currentSection);


            line = sr.ReadLine();
            while(line!=null)
            {
                
                if (line == null)
                    line = "";
                line = line.Trim(); //prepare and clean string

                if (line.Length > 0 && !line.StartsWith(";")) //only process lines containing usefull data (no comments, no empty lines)
                {

                    if (line.StartsWith("[") && line.EndsWith("]")) //start of section detected
                    {
                        string name = line.Substring(1, line.Length - 2);
                        if (!sections.Contains(currentSection))
                            sections.Add(currentSection);

                        //change current section
                        currentSection = sections.FirstOrDefault(f => f.Name == name) ?? new IniSection() { Name = name };
                    }

                    int eqpos = line.IndexOf('=');
                    if (eqpos >= 0 && !line.StartsWith("[")) //valid "key=value" match, add
                    {
                        string key = line.Substring(0, eqpos).Trim();
                        string value = line.Substring(eqpos + 1, line.Length - eqpos - 1).Trim();
                        currentSection[key] = value;
                    }
                }
                line = sr.ReadLine();
            }

            //don´t forget adding the most recent section
            if (!sections.Contains(currentSection))
                sections.Add(currentSection);

            return sections;
        }
    }
}
