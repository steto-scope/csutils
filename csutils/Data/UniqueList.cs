using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace csutils.Data
{
    /// <summary>
    /// Subclass of List&lt;T&gt;. Extends the List-functionality by preventing adding duplicates (similar to HashSet&lt;T&gt;). 
    /// Implements INotifyPropertyChanged that fires when Items are added or removed.
    /// NOTICE: Implementation is based on hiding original methods. Don't use Add/Insert/Remove on an object that is casted to List&lt;T&gt;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UniqueList<T> : List<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Adds an item if not already in the List
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            if (!this.Contains(item))
            {
                base.Add(item);
                OnPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Adds items, that aren't contained in the List
        /// </summary>
        /// <param name="items"></param>
        public new void AddRange(IEnumerable<T> items)
        {
            var subset = this.Union(items).Except(this);
            base.AddRange(subset);

            if (subset.Count() > 0)
                OnPropertyChanged("Count");
        }

        /// <summary>
        /// Inserts an item if it isn't already part of the List
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public new void Insert(int index, T item)
        {
            if (!this.Contains(item))
            {
                base.Insert(index, item);
                OnPropertyChanged("Count");
            }
        }

        /// <summary>
        /// Inserts items that are not already contained in the List
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public new void InsertRange(int index, IEnumerable<T> items)
        {
            var subset = this.Union(items).Except(this);
            base.InsertRange(index, subset);

            if (subset.Count() > 0)
                OnPropertyChanged("Count");
        }

        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            base.Remove(item);
            OnPropertyChanged("Count");
        }

        /// <summary>
        /// Removes a range of items
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            OnPropertyChanged("Count");
        }

        /// <summary>
        /// Removes the item at the given index
        /// </summary>
        /// <param name="index"></param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnPropertyChanged("Count");
        }

        /// <summary>
        /// Removes the items that Match the Predicate
        /// </summary>
        /// <param name="match"></param>
        public new void RemoveAll(Predicate<T> match)
        {
            base.RemoveAll(match);
            OnPropertyChanged("Count");
        }

        /// <summary>
        /// Clears the List
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            OnPropertyChanged("Count");
        }

        /// <summary>
        /// PropertyChanged-Event. Fires whenever Count is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
