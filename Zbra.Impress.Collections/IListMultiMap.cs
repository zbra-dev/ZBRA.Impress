using System.Collections.Generic;

namespace Zbra.Impress.Collections
{

    public interface IListMultiMap<K, V> : IMultiMap<K, V, IList<V>>
    {

        void AddAll(K key, IList<V> all);
    }

}
