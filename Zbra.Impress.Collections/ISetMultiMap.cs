using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public interface ISetMultiMap<K, V> : IMultiMap<K, V, ISet<V>>
    {
        void AddAll(K key, ISet<V> all);
    }
}
