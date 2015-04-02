using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace csutils.Globalisation.TranslationProvider
{
    /// <summary>
    /// Interface for translation sources
    /// </summary>
    public interface ITranslationProvider
    {
        
        /// <summary>
        /// converts the content into a dictionary with the translations. 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Dictionary<string, string> Parse(string content);
        /// <summary>
        /// converts the content into a dictionary with the translations.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Dictionary<string, string> Parse(Stream content);

    }
}
