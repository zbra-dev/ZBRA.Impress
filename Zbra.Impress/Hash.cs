using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress
{
    public class Hash
    {
        private int calculatedHash;
        private readonly static int alpha = 0;
        private readonly static int beta = 23;

        public static Hash Create(object obj)
        {
            return new Hash(alpha).Add(obj);
        }

        public static Hash Create<T>(IEnumerable<T> enumerable)
        {
            return new Hash(alpha).Add(enumerable);
        }

        private Hash(int hashCode)
        {
            calculatedHash = hashCode;
        }

        public Hash Add(object obj)
        {
            return obj == null ? this : new Hash(Rehash(calculatedHash.GetHashCode(), obj.GetHashCode()));
        }

        public Hash Add<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return this;
            }

            int loopHash = this.calculatedHash;
            foreach (var element in enumerable)
            {
                loopHash = Rehash(loopHash, element.GetHashCode());
            }
            return new Hash(loopHash);
        }

        public override bool Equals(object obj)
        {
            var that = obj as Hash;
            return (that != null && that.calculatedHash == this.calculatedHash);
        }

        public override int GetHashCode()
        {
            return calculatedHash;
        }

        private int Rehash(int hash, int newValue)
        {
            return unchecked(hash * beta + newValue);
        }
    }
}
