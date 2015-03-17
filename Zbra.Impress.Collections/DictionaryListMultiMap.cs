using System.Collections.Generic;
using System.Linq;

namespace Zbra.Impress.Collections
{
    /// <summary>
    /// This implementations is not synchronized.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class DictionaryListMultiMap<K, V> : AbstractDictionaryMultiMap<K, V, IList<V>>, IListMultiMap<K, V>
    {

        public DictionaryListMultiMap() { }

        public DictionaryListMultiMap(int capacity) : base(capacity) { }

        public DictionaryListMultiMap(IListMultiMap<K, V> other)
        {
            this.AddAll(other);
        }

        protected override IList<V> newInstance()
        {
            return new List<V>();
        }

        protected override bool Contains(V value, IList<V> collection)
        {
            return collection.Contains(value);
        }

        public void AddAll(K key, System.Collections.Generic.IList<V> all)
        {
            if (all.Any())
            {
                GetCollection(key).AddRange(all);
            }
        }
    }
}
