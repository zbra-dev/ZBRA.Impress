using System.Collections.Generic;

namespace ZBRA.Impress.Collections
{
    public interface IRevertableDictionary<K, V> : IDictionary<K, V>
    {

        bool ContainsValue(V value);
        IRevertableDictionary<V, K> Revert();
    }
}
