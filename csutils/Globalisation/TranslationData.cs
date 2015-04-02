using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace csutils.Globalisation
{
    /// <summary>
    /// Weak event listener for translation data
    /// </summary>
    public class TranslationData : IWeakEventListener, INotifyPropertyChanged, IDisposable
    {
        private string _key;
        private string channel;
        private CultureInfo language;

        /// <summary>
        /// Initializes a TranslationData-Object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="channel"></param>
        /// <param name="language"></param>
        public TranslationData(string key, string channel, string language)
        {
            _key = key;
            this.channel = channel;
            if (language != null)
            {
                try
                {
                    this.language = new CultureInfo(language);
                }
                catch
                {
                    this.language = TranslationManager.DefaultCulture;
                }
            }
            LanguageChangedEventManager.AddListener(null, this);
        }

        /// <summary>
        /// Dispose on Deconstruct
        /// </summary>
        ~TranslationData()
        {
            Dispose(false);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the object (removes Listener)
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LanguageChangedEventManager.RemoveListener(null, this);
            }
        }

        /// <summary>
        /// The translated Text
        /// </summary>
        public object Value
        {
            get
            {
                return TranslationManager.Translate(_key,channel??"",language);
            }
        }

        /// <summary>
        /// Gets called if the Language changed
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                OnLanguageChanged(sender, e);
                return true;
            }
            return false;
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        /// <summary>
        /// PropertyChanged Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
