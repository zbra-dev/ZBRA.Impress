using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public abstract class AbstractRevertableBiDictionary<K, V> : IRevertableDictionary<K, V>
    {
        protected internal abstract IDictionary<K, V> Direct { get; }
        protected internal abstract IDictionary<V, K> Reverted { get; }

        protected AbstractRevertableBiDictionary()
        {
        }

        public virtual IRevertableDictionary<V, K> Revert()
        {
            return new RevertedBiDictionary<V, K>(this);
        }

        public void Add(K key, V value)
        {
            Direct.Add(key, value);
            Reverted.Add(value, key);
        }

        public bool ContainsKey(K key)
        {
            return Direct.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            return Reverted.ContainsKey(value);
        }

        public ICollection<K> Keys
        {
            get { return Direct.Keys; }
        }

        public bool Remove(K key)
        {
            if (Direct.ContainsKey(key))
            {
                var value = Direct[key];
                Direct.Remove(key);
                Reverted.Remove(value);
                return true;
            }
            return false;
        }

        public bool TryGetValue(K key, out V value)
        {
            return Direct.TryGetValue(key, out value);
        }

        public ICollection<V> Values
        {
            get { return Reverted.Keys; }
        }

        public V this[K key]
        {
            get
            {
                return Direct[key];
            }
            set
            {
                Direct[key] = value;
            }
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Direct.Add(item);
        }

        public void Clear()
        {
            Direct.Clear();
            Reverted.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return Direct.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            Direct.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Direct.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return this.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return Direct.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Direct.GetEnumerator();
        }
    }
}
