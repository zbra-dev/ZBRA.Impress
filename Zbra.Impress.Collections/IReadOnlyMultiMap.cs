using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zbra.Impress.Collections
{
    public interface IReadOnlyMultiMap<K, V, C> : IEnumerable<KeyValuePair<K, C>> where C : IEnumerable<V>
    {
        IReadOnlyDictionary<K, C> asDictionary();
        int Count { get; }
        C this[K key] { get; }
        Maybe<C> MaybeGet(K key);
        Maybe<C> MaybeGet(Maybe<K> key);
        /// <summary>
        /// Returns the collections of values associated with the key or an empty collection if the does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        C GetOrEmpty(K key);
        void TryGetValue(K key, out C value);
        bool ContainsKey(K key);
        bool ContainsValue(V value);
        bool ContainsValueForKey(K Key, V value);
        IEnumerable<V> Values { get; }
        ISet<K> Keys { get; }

    }
}
