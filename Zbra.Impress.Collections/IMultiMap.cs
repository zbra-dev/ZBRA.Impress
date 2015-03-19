using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Collections
{
    public interface IMultiMap<K, V, C> : IReadOnlyMultiMap<K, V, C> where C : IEnumerable<V>
    {
        void Add(K key, V value);
        void AddAll<U>(K key, IEnumerable<U> all) where U : V;
        void AddAll<U, E>(IMultiMap<K, U, E> other)
            where E : IEnumerable<U>
            where U : V;
        void Clear();
        void ClearKey(K key);
        /// <summary>
        /// Removes all values and key that match the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(K key);
        bool RemoveAllValues(V value);
        bool RemoveValue(K key, V value);

    }

    public static class MultiMapExtentions
    {
        public static IListMultiMap<NK, V> Remap<K, V, NK>(this IListMultiMap<K, V> map, Func<K, NK> rehash)
        {
            var result = new DictionaryListMultiMap<NK, V>();

            foreach (var d in map)
            {
                result.AddAll(rehash(d.Key), d.Value);
            }

            return result;
        }

        public static IListMultiMap<NK, NV> Remap<K, V, NK, NV>(this IListMultiMap<K, V> map, Func<K, NK> rehash, Func<V, NV> revalue)
        {
            var result = new DictionaryListMultiMap<NK, NV>();

            foreach (var d in map)
            {
                result.AddAll(rehash(d.Key), d.Value.Select(revalue));
            }

            return result;
        }
    }
}
