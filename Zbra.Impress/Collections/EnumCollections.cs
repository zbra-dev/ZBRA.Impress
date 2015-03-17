using System;
using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public class EnumCollections
    {

        public static ISet<E> AsSet<E>() where E : struct
        {
            var type = typeof(E);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum");
            }
            return new HashSet<E>((E[])Enum.GetValues(type));
        }

        public static IList<E> AsList<E>() where E : struct
        {
            var type = typeof(E);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum");
            }
            return new List<E>((E[])Enum.GetValues(type));
        }

        public static IReadOnlyList<E> AsReadOnlyList<E>() where E : struct
        {
            var type = typeof(E);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum");
            }
            return (E[])Enum.GetValues(type);
        }

        public static IEnumerable<E> AsEnumerable<E>() where E : struct
        {
            var type = typeof(E);
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum");
            }
            return (E[])Enum.GetValues(type);
        }
    }
}
