using csutils.Globalisation.TranslationProvider;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace csutils.Globalisation
{
    /// <summary>
    /// Factory for TranslationProviders. If you want to add your own Provider derive this class and override the Create()-method.
    /// </summary>
    public class TranslationProviderFactory
    {
        /// <summary>
        /// Creates a TranslationProvider based on a given file. Returns null if no suitable translation provider is found
        /// </summary>
        /// <param name="file">FileInfo that describes the source</param>
        /// <returns>null, if no suitable provider can be found</returns>
        public ITranslationProvider Create(FileInfo file)
        {
            if (file==null || !file.Exists)
                throw new ArgumentException("argument has to be an existing file");

            return Create(file.Extension);            
        }

        /// <summary>
        /// Creates a translation provider based on a extension. Override this method to add more providers
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public virtual ITranslationProvider Create(string extension)
        {
            if (extension == null )
                throw new ArgumentException("extension has to be a file extension (like .xyz) ");

            if (extension.ToLower() == ".xml")
            {
                var xmlprovider = new XmlTranslationProvider();
                return xmlprovider;
            }
            return null;
        }

    }
}
