using System;
using System.Collections;
using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public class ConcurrentDictionary<K, V> : IConcurrentDictionary<K, V>
    {
        private IDictionary<K, V> dictionary;
        private object dictionaryLock = new Object();

        public ConcurrentDictionary(IDictionary<K, V> dictionary)
        {
            this.dictionary = dictionary;
        }

        #region IDictionary<K, V> Implementation

        void IDictionary<K, V>.Add(K key, V value)
        {
            lock (dictionaryLock)
            {
                dictionary.Add(key, value);
            }
        }

        bool IDictionary<K, V>.ContainsKey(K key)
        {
            lock (dictionaryLock)
            {
                return dictionary.ContainsKey(key);
            }
        }

        bool IDictionary<K, V>.Remove(K key)
        {
            lock (dictionaryLock)
            {
                return dictionary.Remove(key);
            }
        }

        bool IDictionary<K, V>.TryGetValue(K key, out V value)
        {
            lock (dictionaryLock)
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        V IDictionary<K, V>.this[K key]
        {
            get
            {
                lock (dictionaryLock)
                {
                    return dictionary[key];
                }
            }
            set
            {
                lock (dictionaryLock)
                {
                    dictionary[key] = value;
                }
            }
        }

        ICollection<K> IDictionary<K, V>.Keys
        {
            get
            {
                lock (dictionaryLock)
                {
                    return dictionary.Keys;
                }
            }
        }

        ICollection<V> IDictionary<K, V>.Values
        {
            get
            {
                lock (dictionaryLock)
                {
                    return dictionary.Values;
                }
            }
        }

        void ICollection<KeyValuePair<K, V>>.Add(KeyValuePair<K, V> item)
        {
            lock (dictionaryLock)
            {
                dictionary.Add(item);
            }
        }

        void ICollection<KeyValuePair<K, V>>.Clear()
        {
            lock (dictionaryLock)
            {
                dictionary.Clear();
            }
        }

        bool ICollection<KeyValuePair<K, V>>.Contains(KeyValuePair<K, V> item)
        {
            lock (dictionaryLock)
            {
                return dictionary.Contains(item);
            }
        }

        void ICollection<KeyValuePair<K, V>>.CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            lock (dictionaryLock)
            {
                dictionary.CopyTo(array, arrayIndex);
            }
        }

        int ICollection<KeyValuePair<K, V>>.Count
        {
            get
            {
                lock (dictionaryLock)
                {
                    return dictionary.Count;
                }
            }
        }

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly
        {
            get
            {
                lock (dictionaryLock)
                {
                    return dictionary.IsReadOnly;
                }
            }
        }

        bool ICollection<KeyValuePair<K, V>>.Remove(KeyValuePair<K, V> item)
        {
            lock (dictionaryLock)
            {
                return dictionary.Remove(item);
            }
        }

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            lock (dictionaryLock)
            {
                return dictionary.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (dictionaryLock)
            {
                return dictionary.GetEnumerator();
            }
        }
        #endregion

        public V GetOrAdd(K key, Func<K, V> addPredicate)
        {
            lock (dictionaryLock)
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                else
                {
                    var newValue = addPredicate(key);
                    dictionary.Add(key, newValue);
                    return newValue;
                }
            }
        }
    }
}
