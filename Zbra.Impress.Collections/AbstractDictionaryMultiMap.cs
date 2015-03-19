using System;
using System.Collections.Generic;
using System.Linq;

namespace ZBRA.Impress.Collections
{
    public abstract class AbstractDictionaryMultiMap<K, V, C> : IMultiMap<K, V, C> where C : ICollection<V>
    {
        private Dictionary<K, C> items;

        protected AbstractDictionaryMultiMap()
        {
            items = new Dictionary<K, C>();
        }

        protected AbstractDictionaryMultiMap(int capacity)
        {
            items = new Dictionary<K, C>(capacity);
        }

        protected C GetCollection(K key)
        {
            C collection = default(C);
            if (items.TryGetValue(key, out collection))
            {
                return collection;
            }

            collection = newInstance();
            items.Add(key, collection);

            return collection;
        }

        protected abstract C newInstance();

        public void Add(K key, V value)
        {
            // add the value to the collection. A list will repeat the value, a set will not. 
            // no implementation diference
            GetCollection(key).Add(value);
        }

        public void AddAll<U>(K key, params U[] values) where U : V
        {
            if (values.Length > 0)
            {
                C collection = GetCollection(key);
                foreach (V v in values)
                {
                    collection.Add(v);
                }
            }
        }

        public void AddAll<U>(K key, System.Collections.Generic.IEnumerable<U> all) where U : V
        {
            if (all.Any())
            {
                C collection = GetCollection(key);
                foreach (U obj in all)
                {
                    collection.Add(obj);
                }
            }
        }

        public void AddAll<U, E>(IMultiMap<K, U, E> other)
            where E : System.Collections.Generic.IEnumerable<U>
            where U : V
        {
            foreach (var pair in other.asDictionary())
            {
                C collection = GetCollection(pair.Key);
                foreach (V v in pair.Value)
                {
                    collection.Add(v);
                }
            }

        }

        public void Clear()
        {
            items.Clear();
        }

        public void ClearKey(K key)
        {
            if (items.ContainsKey(key))
            {
                items.Remove(key);
                items.Add(key, newInstance());
            }
        }

        public bool Remove(K key)
        {
            return items.Remove(key);
        }

        public bool RemoveAllValues(V value)
        {
            var removed = false;

            foreach (var pair in items)
            {
                removed = removed | pair.Value.Remove(value);// Do not use short-circuit
            }
            return removed;
        }

        public bool RemoveValue(K key, V value)
        {
            C collection = default(C);
            if (items.TryGetValue(key, out collection))
            {
                return collection.Remove(value);
            }
            return false;
        }

        public System.Collections.Generic.IReadOnlyDictionary<K, C> asDictionary()
        {
            return items;
        }

        public int Count
        {
            get { return items.Count; }
        }

        public C this[K key]
        {
            get
            {
                C collection = default(C);
                if (items.TryGetValue(key, out collection))
                {
                    return collection;
                }
                throw new Exception("Elements does not exist"); // TODO use correct exception
            }
        }

        public Maybe<C> MaybeGet(K key)
        {
            C collection = default(C);
            if (items.TryGetValue(key, out collection))
            {
                return collection.ToMaybe();
            }
            return Maybe<C>.Nothing;
        }


        public Maybe<C> MaybeGet(Maybe<K> key)
        {
            if (!key.HasValue)
            {
                return Maybe<C>.Nothing;
            }
            else
            {
                return this.MaybeGet(key.Value);
            }

        }

        public C GetOrEmpty(K key)
        {
            C collection = default(C);
            if (items.TryGetValue(key, out collection))
            {
                return collection;
            }
            return newInstance();
        }

        public void TryGetValue(K key, out C collection)
        {
            items.TryGetValue(key, out collection);
        }

        public bool ContainsKey(K key)
        {
            return items.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            foreach (var pair in items)
            {
                if (Contains(value, pair.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsValueForKey(K key, V value)
        {
            C collection = default(C);
            if (items.TryGetValue(key, out collection))
            {
                return Contains(value, collection); ;
            }
            return false;
        }

        protected abstract bool Contains(V value, C collection);

        public System.Collections.Generic.IEnumerable<V> Values
        {
            get
            {
                var result = Enumerable.Empty<V>();
                foreach (var pair in items)
                {
                    result = result.Concat(pair.Value.AsEnumerable<V>());
                }
                return result;
            }
        }

        public System.Collections.Generic.ISet<K> Keys
        {
            get
            {
                return items.Keys.ToSet(); // Creates set and copies keys so has readonly semantics
            }
        }

        private IEnumerator<KeyValuePair<K, C>> DoGetEnumerator()
        {
            return items.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<K, C>> GetEnumerator()
        {
            return DoGetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return DoGetEnumerator();
        }

    }
}
