using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Data
{
    /// <summary>
    /// Base class for data objects. Provides an implementation of INotifyPropertyChanged, ICloneable and Serialisation
    /// </summary>
    [Serializable]
    public abstract class Base : INotifyPropertyChanged, ICloneable
    {
        #region Privates

        /// <summary>
        /// the internal dictionary that stores the properties
        /// </summary>
        protected IDictionary<string, object> values = new SerializableDictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

        #endregion

        #region Methods
        /// <summary>
        /// Gets value (casted)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetProperty<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);

            var value = this.GetProperty(key);

            if (value is T)
                return (T)value;

            return default(T);
        }
        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetProperty(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            if (this.values.ContainsKey(key))
                return this.values[key];

            return null;
        }
        /// <summary>
        /// Sets value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetProperty(string key, object value)
        {
            if (!this.values.ContainsKey(key))
                this.values.Add(key, value);
            else
                this.values[key] = value;

            OnPropertyChanged(key);
        }

        /// <summary>
        /// Sets a value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public virtual void Set(string key, object value)
        {
            SetProperty(key, value);
        }
        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(string key)
        {
            return GetProperty(key);
        }
        /// <summary>
        /// Gets a value (casted)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            try
            {
                object o = GetProperty(key);
                if (o == null)
                    return default(T);
                return (T)o;
            }
            catch
            {
                return default(T);
            }
        }


        #endregion
        /// <summary>
        /// PropertyChanged-Event (INotifyPropertyChanged)
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes PropertyChanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// Invoke notification that all properties changed
        /// </summary>
        public void FireAllPropertiesChanged()
        {
            foreach (string key in values.Keys)
                OnPropertyChanged(key);
        }


        /// <summary>
        /// Clones and returns a casted object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Clone<T>() where T : Base, new()
        {
            T newobj = new T();

            foreach (string key in values.Keys)
            {
                object content = values[key];
                if (content != null)
                {
                    Type t = content.GetType();

                    if (t != null && t.IsValueType)
                    {
                        newobj.SetProperty(key, content);
                    }
                    else if (t != null && !t.IsValueType && content is ICloneable)
                    {
                        newobj.SetProperty(key, ((ICloneable)content).Clone());
                    }
                    else
                        newobj.SetProperty(key, content);
                }
                else
                    newobj.SetProperty(key, content);
            }
            return newobj;
        }


        /// <summary>
        /// Implement clone by calling base.Clone&lt;YourType&gt;();
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();
    }
}
