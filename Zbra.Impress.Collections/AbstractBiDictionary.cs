using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public class AbstractBiDictionary<K, V> : AbstractRevertableBiDictionary<K, V>
    {
        private IDictionary<K, V> direct;
        private IDictionary<V, K> reversed;

        protected AbstractBiDictionary(IDictionary<K, V> direct, IDictionary<V, K> reversed)
        {
            this.direct = direct;
            this.reversed = reversed;
        }

        protected internal override System.Collections.Generic.IDictionary<K, V> Direct
        {
            get { return direct; }
        }

        protected internal override System.Collections.Generic.IDictionary<V, K> Reverted
        {
            get { return reversed; }
        }
    }
}
