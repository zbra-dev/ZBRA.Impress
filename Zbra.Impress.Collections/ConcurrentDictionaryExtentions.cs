using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public static class ConcurrentDictionaryExtentions
    {
        public static IConcurrentDictionary<K, V> ToConcurrent<K, V>(this IDictionary<K, V> dictionary)
        {
            return new ConcurrentDictionary<K, V>(dictionary);
        }
    }
}
