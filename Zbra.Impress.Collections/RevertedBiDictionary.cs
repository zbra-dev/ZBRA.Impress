using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    internal class RevertedBiDictionary<V, K> : AbstractRevertableBiDictionary<V, K>
    {
        private AbstractRevertableBiDictionary<K, V> original;

        public RevertedBiDictionary(AbstractRevertableBiDictionary<K, V> original)
        {
            this.original = original;
        }

        protected internal override IDictionary<V, K> Direct
        {
            get { return original.Reverted; }
        }

        protected internal override IDictionary<K, V> Reverted
        {
            get { return original.Direct; }
        }

        public override IRevertableDictionary<K, V> Revert()
        {
            return original;
        }
    }
}
