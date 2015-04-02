using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Data
{
    /// <summary>
    /// Implements a serializable dictionary for paired key dictionary. It´s a wrapper class for  SerializableDictionary&lt;Tuple&lt;T1,T2&gt;,T3&gt;
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class TwoKeyDictionary<T1,T2,T3> : SerializableDictionary<Tuple<T1,T2>,T3>
    {
        /// <summary>
        /// Adds a new entry
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        public void Add(T1 key1, T2 key2, T3 value)
        {
            base.Add(new Tuple<T1, T2>(key1, key2),value);
        }

        /// <summary>
        /// Checks if entry exists based on the keys
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public bool ContainsKey(T1 key1, T2 key2)
        {
            return base.ContainsKey(new Tuple<T1, T2>(key1, key2));
        }

        /// <summary>
        /// Checks if there´s any entry that have the given first key
        /// </summary>
        /// <param name="key1"></param>
        /// <returns></returns>
        public bool ContainsKey1(T1 key1)
        {
            return base.Keys.Select(s => s.Item1).Contains(key1);
        }
        /// <summary>
        /// Checks if there´s any entry that have the given second key
        /// </summary>
        /// <param name="key2"></param>
        /// <returns></returns>
        public bool ContainsKey2(T2 key2)
        {
            return base.Keys.Select(s => s.Item2).Contains(key2);
        }

        /// <summary>
        /// Removes an entry
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public bool Remove(T1 key1, T2 key2)
        {
            return base.Remove(new Tuple<T1, T2>(key1, key2));
        }
        /// <summary>
        /// Removes all keys that have the given first key
        /// </summary>
        /// <param name="key1"></param>
        /// <returns></returns>
        public bool RemoveKey1(T1 key1)
        {
            List<Tuple<T1, T2>> todelete = new List<Tuple<T1, T2>>();
            foreach(Tuple<T1,T2> key in Keys.Where(w=>this.ContainsKey1(w.Item1)))
                todelete.Add(key);

            List<bool> results = new List<bool>();
            foreach (var key in todelete)
                results.Add(base.Remove(key));

            return results.All(a => true);
        }
        /// <summary>
        /// Removes all keys that have the given second key
        /// </summary>
        /// <param name="key2"></param>
        /// <returns></returns>
        public bool RemoveKey2(T2 key2)
        {
            List<Tuple<T1, T2>> todelete = new List<Tuple<T1, T2>>();
            foreach (Tuple<T1, T2> key in Keys.Where(w => this.ContainsKey2(w.Item2)))
                todelete.Add(key);

            List<bool> results = new List<bool>();
            foreach (var key in todelete)
                results.Add(base.Remove(key));

            return results.All(a => true);
        }
        /// <summary>
        /// Tries to get the value
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(T1 key1, T2 key2, out T3 value)
        {
            return TryGetValue(new Tuple<T1, T2>(key1, key2), out value);
        }

        /// <summary>
        /// Access entries by the given keys
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public T3 this[T1 key1, T2 key2]
        {
            get
            {
                return base[new Tuple<T1, T2>(key1, key2)];
            }
            set
            {
                base[new Tuple<T1, T2>(key1, key2)] = value;
            }
        }

        
        
    }
}
