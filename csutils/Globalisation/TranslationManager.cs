using csutils.Data;
using csutils.Globalisation.TranslationProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace csutils.Globalisation
{
    /// <summary>
    /// Manages the translation of text.
    /// 
    /// The Manager allows to create multiple dictionaries for the same language, so called "channels". 
    /// Every channel (identified by a string) can hold 1 dictionary of any language. The channels are independen from each other.
    /// Thus, the manager can be used to create translations of multiple components or for multiple purposes.
    /// For example there can be a set of translations (en, de, es) for the UI components´ text and another set for detailed help / tooltips.
    /// </summary>
    public sealed class TranslationManager 
    {

        /// <summary>
        /// stores the data: (languagecode, channelname) -> Dictionary ( word -> translation)
        /// </summary>
        private static TwoKeyDictionary<string, string, Dictionary<string, string>> dictionaries = new TwoKeyDictionary<string, string, Dictionary<string, string>>();

        /// <summary>
        /// holds the factory
        /// </summary>
        private static TranslationProviderFactory tpfactory = new TranslationProviderFactory();

        /// <summary>
        /// Gets or sets the TranslationProviderFactory
        /// </summary>
        public static TranslationProviderFactory TranslationProviderFactory
        {
            get
            {
                return tpfactory;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("value of TranslationProviderFactory can not be null");

                tpfactory = value;
            }
        }

        private static CultureInfo defaultCulture = new CultureInfo("en-US");

        /// <summary>
        /// Gets or sets the default culture. This is the fallback culture if no translation can be found for the primary one
        /// </summary>
        public static CultureInfo DefaultCulture
        {
            get
            {
                return defaultCulture;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("value of DefaultCulture can not be null");

                defaultCulture = value;
            }
        }

        /// <summary>
        /// Gets fired if the CurrentCulture gets changed
        /// </summary>
        public static event EventHandler LanguageChanged;


        private static CultureInfo currentculture = new CultureInfo("en-US");
        /// <summary>
        /// User defined Culture. This culture has priority.
        /// Set to null or to the same value has no effect, other will trigger LanguageChanged-event
        /// </summary>
        public static CultureInfo CurrentCulture
        {
            get { return currentculture; }
            set
            {
                if (value == null) //null is no valid culture
                    throw new Exception("value of CurrentCulture can not be null");

                if (currentculture != value) //prevent firing LanguageChanged when there is no real change
                {
                    currentculture = value;

                    if (LanguageChanged != null)
                        LanguageChanged(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Manually invokes the LanguageChanged event. 
        /// </summary>
        public static void FireLanguageChanged()
        {
            if (LanguageChanged != null)
                LanguageChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the installed Translations
        /// </summary>
        /// <param name="channel">only translation dictionaries with the specified name</param>
        /// <returns></returns>
        public static IEnumerable<string> InstalledTranslations(string channel = "")
        {
            return dictionaries.Where(w => w.Key.Item2 == channel).Select(s => s.Key.Item1);
        }

        /// <summary>
        /// Adds a translation given by a file. 
        /// If the culture-parameter is null the method will try to determine the culture by itself based on the filename.
        /// This check succeeds if the filename contains a string that looks like a culture-code: xx-XX.
        /// If the culture is not given and can not be determined the method will throw an esception
        /// </summary>
        /// <param name="file">file that contains the translation</param>
        /// <param name="culture">the language the content translates into. If null the method will try to determine it by the filename</param>
        /// <param name="channel">an optional name for this set of translations</param>
        public static void Add(string file, CultureInfo culture = null, string channel = "")
        {
            if (channel == null)
                throw new ArgumentException("name can not be null");
            if (file == null && File.Exists(file))
                throw new ArgumentException("file has to be an existing file");

            if (culture == null)
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                foreach (Match m in Regex.Matches(file, @"\b\w\w(\-\w\w)?\b",RegexOptions.RightToLeft))
                {
                    culture = cultures.FirstOrDefault(f => f.Name == m.Value);
                    if (culture != null)
                        break;
                }
            }

            if (culture == null)
                throw new ArgumentException("the culture can not be determined by the filename. Rename the file that it contains a culture-name like 'en-US' or specify the culture manually by using the culture-parameter");

            Add(File.OpenRead(file), new FileInfo(file).Extension, culture, channel);
        }

        /// <summary>
        /// Adds a translation given by a stream. 
        /// </summary>
        /// <param name="stream">the stream with the content</param>
        /// <param name="extension">determines the data format of the stream content (for example: .xml)</param>
        /// <param name="culture">the language the content translates into</param>
        /// <param name="channel">an optional name for this set of translations</param>
        public static void Add(Stream stream, string extension, CultureInfo culture, string channel = "")
        {
            if (channel == null)
                throw new ArgumentException("name can not be null");
            if (culture == null)
                throw new ArgumentException("culture can not be null");

            ITranslationProvider provider = TranslationProviderFactory.Create(extension);
            if (provider == null)
                throw new Exception("For this data format exists no Translation provider");

            var dict = provider.Parse(stream);

            dictionaries[culture.TwoLetterISOLanguageName, channel] =  dict;
        }

        /// <summary>
        /// clears the stored translations
        /// </summary>
        public static void Clear()
        {
            dictionaries.Clear();
        }

        /// <summary>
        /// translates a string into another language. 
        /// Tries to get the translation for the target culture. 
        /// If not possible, the DefaultCulture. 
        /// If still not possible the method returns the key itself (no action taken).
        /// </summary>
        /// <param name="key">the key-string</param>
        /// <param name="channel">name of the dictionary</param>
        /// <param name="targetCulture">the target culture. if null CurrentCulture is used</param>
        /// <returns></returns>
        public static string Translate(string key, string channel = "", CultureInfo targetCulture = null)
        {
            if (channel == null)
                throw new ArgumentException("name can not be null");
            if (key == null)
                return null;

            if (targetCulture == null)
                targetCulture = CurrentCulture;

            if(dictionaries.ContainsKey(targetCulture.TwoLetterISOLanguageName,channel) && dictionaries[targetCulture.TwoLetterISOLanguageName, channel].ContainsKey(key))
            {
                return dictionaries[targetCulture.TwoLetterISOLanguageName, channel][key];
            }
            else if(dictionaries.ContainsKey(DefaultCulture.TwoLetterISOLanguageName,channel)&& dictionaries[DefaultCulture.TwoLetterISOLanguageName, channel].ContainsKey(key))
            {
                return dictionaries[DefaultCulture.TwoLetterISOLanguageName, channel][key];
            }
            else
            {
                return key;
            }

        }

    }
}
