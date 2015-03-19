using System.Collections.Generic;

namespace ZBRA.Impress.Collections
{
    /// <summary>
    /// This implementations is not synchronized.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class DictionarySetMultiMap<K, V> : AbstractDictionaryMultiMap<K, V, ISet<V>>, ISetMultiMap<K, V>
    {

        public DictionarySetMultiMap() { }

        public DictionarySetMultiMap(int capacity) : base(capacity) { }

        protected override ISet<V> newInstance()
        {
            return new HashSet<V>();
        }

        protected override bool Contains(V value, ISet<V> collection)
        {
            return collection.Contains(value);
        }

        public void AddAll(K key, System.Collections.Generic.ISet<V> all)
        {
            if (all.Count > 0)
            {
                GetCollection(key).UnionWith(all);
            }
        }
    }
}
