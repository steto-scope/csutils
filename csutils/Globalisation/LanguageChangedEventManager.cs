using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace csutils.Globalisation
{
    /// <summary>
    /// Weak event manager for changed languages
    /// </summary>
    public class LanguageChangedEventManager : WeakEventManager
    {
        /// <summary>
        /// Adds listener
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listener"></param>
        public static void AddListener(TranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Removes Listener
        /// </summary>
        /// <param name="source"></param>
        /// <param name="listener"></param>
        public static void RemoveListener(TranslationManager source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            DeliverEvent(sender, e);
        }

        /// <summary>
        /// Starts listening
        /// </summary>
        /// <param name="source"></param>
        protected override void StartListening(object source)
        {
            TranslationManager.LanguageChanged += OnLanguageChanged;
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        /// <param name="source"></param>
        protected override void StopListening(Object source)
        {
            TranslationManager.LanguageChanged -= OnLanguageChanged;
        }

        /// <summary>
        /// gets current weak event manager
        /// </summary>
        private static LanguageChangedEventManager CurrentManager
        {
            get
            {
                Type managerType = typeof(LanguageChangedEventManager);
                var manager = (LanguageChangedEventManager)GetCurrentManager(managerType);
                if (manager == null)
                {
                    manager = new LanguageChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }
    }
}
