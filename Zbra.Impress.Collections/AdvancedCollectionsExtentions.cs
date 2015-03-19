using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Collections
{
    public static class AdvancedCollectionsExtentions
    {
        public static ISetMultiMap<K, V> OrEmpty<K, V>(this Maybe<ISetMultiMap<K, V>> maybe)
        {
            return maybe.Or(EmptySetMultiMap<K, V>());
        }

        public static IListMultiMap<K, V> OrEmpty<K, V>(this Maybe<IListMultiMap<K, V>> maybe)
        {
            return maybe.Or(EmptyListMultiMap<K, V>());
        }

        public static IListMultiMap<K, V> EmptyListMultiMap<K, V>()
        {
            return new DictionaryListMultiMap<K, V>(0);
        }

        public static ISetMultiMap<K, V> EmptySetMultiMap<K, V>()
        {
            return new DictionarySetMultiMap<K, V>(0);
        }

        public static IListMultiMap<K, V> ToGroupingDictionary<K, V>(this IEnumerable<IGrouping<K, V>> data)
        {

            var dictionary = new DictionaryListMultiMap<K, V>();
            foreach (IGrouping<K, V> g in data)
            {
                dictionary.AddAll(g.Key, g.AsEnumerable());
            }

            return dictionary;
        }

        public static IListMultiMap<K, V> GroupByList<K, V>(this IQueryable<V> data, Func<V, K> groupMap)
        {
            var list = data.ToList().GroupBy(groupMap);
            var dictionary = new DictionaryListMultiMap<K, V>();
            foreach (IGrouping<K, V> g in list)
            {
                dictionary.AddAll(g.Key, g.AsEnumerable());
            }

            return dictionary;
        }

        public static IListMultiMap<K, T> ToGroupingDictionary<K, V, T>(this IEnumerable<IGrouping<K, V>> data, Func<V, T> transform)
        {
            var dictionary = new DictionaryListMultiMap<K, T>();
            foreach (IGrouping<K, V> g in data)
            {
                dictionary.AddAll(g.Key, g.Select(transform));
            }
            return dictionary;
        }

    }
}
