using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace csutils.Globalisation.TranslationProvider
{
    /// <summary>
    /// Xml-based translation source.
    /// The content should contain 1 root element (name is ignored), which has x children. 
    /// The child´s name will be the key, the content (or the ValueAttributeName attribute) will the translation
    /// </summary>
    /// <example>
    /// 
    ///  &lt;?xml version="1.0" &gt;
    ///  &lt;root&gt;
    ///     &lt;element&gt;translated text&lt;/element&gt;
    ///     &lt;element2 value="second translated text" /&gt;
    ///  &lt;/root&gt;
    ///  
    /// </example>
    public class XmlTranslationProvider : ITranslationProvider
    {
        /// <summary>
        /// Name of the xml attribute that contains the translation if a selfclosing tag is used. Default is "value"
        /// </summary>
        public string ValueAttributeName { get; set; }

        /// <summary>
        /// create new provider
        /// </summary>
        public XmlTranslationProvider()
        {
            ValueAttributeName = "value";
        }

        /// <summary>
        /// string that holds the content
        /// </summary>
        private string content;

        /// <summary>
        /// parses the source into a Dictionary
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> Parse()
        {            

            Dictionary<string, string> dict = new Dictionary<string, string>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(content);
            if (xmldoc.HasChildNodes)
            {
                XmlNode root = xmldoc.DocumentElement;
                foreach (XmlNode n in root.ChildNodes)
                {
                    string name = n.Name;
                    string val;
                    if (n.Attributes[ValueAttributeName] != null)
                        val = n.Attributes[ValueAttributeName].Value;
                    else
                        val = n.InnerText;

                    if(val!=null && val.Length>0)
                        dict[name] = val;
                }
            }
            return dict;
        }
        
        /// <inheritdoc />
        public Dictionary<string, string> Parse(string content)
        {
            this.content = content;
            return Parse();
        }

        /// <summary>
        /// reads the content out of the stream (using Unicode). The stream gets closed after reading.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Dictionary<string, string> Parse(Stream content)
        {
            if (content == null)
                throw new Exception("content can not be null");

            if (content.CanSeek && content.Position > 0)
                content.Seek(0, SeekOrigin.Begin);

            using(StreamReader sr = new StreamReader(content,Encoding.Unicode))
            {
                this.content = sr.ReadToEnd();
            }
            return Parse();
        }
    }
}
