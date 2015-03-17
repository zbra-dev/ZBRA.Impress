using System;
using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public interface IConcurrentDictionary<K, V> : IDictionary<K, V>
    {
        V GetOrAdd(K key, Func<K, V> addPredicate);
    }
}
