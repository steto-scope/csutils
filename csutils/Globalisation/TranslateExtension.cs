#if CLR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace csutils.Globalisation
{
    /// <summary>
    /// Translates a string
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        private string _key;

        /// <summary>
        /// Creates a new translate object
        /// </summary>
        /// <param name="key"></param>
        public TranslateExtension(string key)
        {
            _key = key;
        }

        /// <summary>
        /// key to search
        /// </summary>
        [ConstructorArgument("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string language;
        /// <summary>
        /// Target language code. For example 'de' or 'de-DE'. If not set, TranslationManager.CurrentCulture is used
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        private string channel;

        /// <summary>
        /// Specifies the name of the translation channel. See <see cref="TranslationManager"/> for more details
        /// </summary>
        public string Channel
        {
            get { return channel; }
            set { channel = value; }
        }


        /// <summary>
        /// method that obtains the data
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new TranslationData(_key,channel,language)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
#endif