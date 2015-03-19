﻿using System.Collections.Generic;

namespace ZBRA.Impress.Collections
{
    public static class SetExtentions
    {

        /// <summary>
        /// Returns an immutable IList with zero elements. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An immutable ISet with zero elements. </returns>
        public static ISet<T> EmptySet<T>()
        {
            return new EmptySet<T>();
        }


        public static ISet<T> OrEmpty<T>(this Maybe<ISet<T>> maybe)
        {
            return maybe.Or(SetExtentions.EmptySet<T>());
        }

        public static ISet<T> ExceptWith<T>(this ISet<T> other, Maybe<T> element)
        {
            if (element.HasValue)
            {
                var set = new HashSet<T>(other);
                set.Remove(element.Value);
                return set;
            }
            else
            {
                return other;
            }
        }
    }
}
