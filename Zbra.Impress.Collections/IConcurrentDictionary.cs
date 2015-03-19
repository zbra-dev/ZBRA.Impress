using System;
using System.Collections.Generic;

namespace ZBRA.Impress.Collections
{
    public interface IConcurrentDictionary<K, V> : IDictionary<K, V>
    {
        V GetOrAdd(K key, Func<K, V> addPredicate);
    }
}
