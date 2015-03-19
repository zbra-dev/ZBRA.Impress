using System.Collections.Generic;

namespace ZBRA.Impress.Collections
{
    public class BiDictionary<K, V> : AbstractBiDictionary<K, V>
    {

        public BiDictionary()
            : base(new Dictionary<K, V>(), new Dictionary<V, K>())
        { }

        public BiDictionary(IEnumerable<KeyValuePair<K, V>> other)
            : this()
        {
            foreach (var pair in other)
            {
                this.Add(pair.Key, pair.Value);
            }
        }
    }
}
