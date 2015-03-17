using System.Collections.Generic;
using Zbra.Impress.Collections;
using System.Linq;

namespace Zbra.Impress.Math
{
    public static class FractionExtensions
    {
        public static Fraction Sum(this IEnumerable<Fraction> source)
        {
            return source.Aggregate(Fraction.Zero, (a, b) => a + b);
        }

        public static Fraction Over(this int value, int other)
        {
            return other == 0 ? Fraction.Zero : Fraction.ValueOf(value, other);
        }

        public static Fraction Over(this long value, long other)
        {
            return other == 0L ? Fraction.Zero : Fraction.ValueOf(value, other);
        }
    }
}
