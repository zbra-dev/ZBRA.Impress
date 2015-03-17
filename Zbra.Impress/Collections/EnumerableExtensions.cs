using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zbra.Impress.Collections
{
    public static class EnumerableExtensions
    {

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<Nullable<T>> enumerable) where T : struct
        {
            return enumerable == null ? Maybe<T>.Nothing : enumerable.SingleOrDefault<Nullable<T>>().ToMaybe<T>();
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
            {
                return Maybe<T>.Nothing;
            }

            T value = enumerable.SingleOrDefault();
            if (typeof(T).IsValueType && value.Equals(default(T)))
            {
                return Maybe<T>.Nothing;
            }
            return value.ToMaybe();
        }

        public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return enumerable == null ? Maybe<T>.Nothing : enumerable.Where(predicate).MaybeSingle();
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable) where T : class
        {
            return enumerable == null ? Maybe<T>.Nothing : enumerable.FirstOrDefault().ToMaybe();
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : class
        {
            return enumerable == null ? Maybe<T>.Nothing : enumerable.Where(predicate).MaybeFirst();
        }

        public static Maybe<T> MaybeFirst<T>(this IEnumerable<Nullable<T>> enumerable) where T : struct
        {
            return enumerable == null ? Maybe<T>.Nothing : enumerable.FirstOrDefault<Nullable<T>>().ToMaybe<T>();
        }

        public static IEnumerable<T> OrEmpty<T>(this Maybe<IEnumerable<T>> maybe)
        {
            return maybe.Or(Enumerable.Empty<T>());
        }

        public static IEnumerable<T> Change<T>(this IEnumerable<T> elements, Action<T> action)
        {
            return elements.Select(m => { action(m); return m; });
        }

        public static IEnumerable<Tuple<K, V>> Associate<K, V>(this IEnumerable<K> elements, IDictionary<K, V> dictionary)
        {
            return Associate(elements, dictionary, e => e, (o, m) => Tuple.Create(o, m));
        }

        public static IEnumerable<Tuple<T, V>> Associate<T, K, V>(this IEnumerable<T> elements, IDictionary<K, V> dictionary, Func<T, K> mapping)
        {
            return Associate(elements, dictionary, mapping, (o, m) => Tuple.Create(o, m));
        }

        public static IEnumerable<R> Associate<T, K, V, R>(this IEnumerable<T> elements, IDictionary<K, V> dictionary, Func<T, K> mapping, Func<T, V, R> constructor)
        {
            if (dictionary.Count == 0)
            {
                yield break;
            }

            foreach (var element in elements)
            {
                V value;
                if (dictionary.TryGetValue(mapping(element), out value))
                {
                    yield return constructor(element, value);
                }
            }
        }

        public static IEnumerable<T> ExcludeNothing<T>(this IEnumerable<Maybe<T>> elements)
        {
            return elements.Where(m => m.HasValue).Select(m => m.Value);
        }

        public static IEnumerable<S> ExcludeNothing<S>(this IEnumerable<Nullable<S>> elements) where S : struct
        {
            return elements.Where(m => m.HasValue).Select(m => m.Value);
        }

        public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> elements)
        {
            if (elements == null)
            {
                return new List<T>();
            }
            return elements;
        }

        public static IEnumerable<S> Convert<S>(this IEnumerable<string> elements) where S : struct
        {
            foreach (var s in elements)
            {
                yield return GenericTypeConverter.GetConverter().Convert<S>(s);
            }
        }

        public static IEnumerable<IGrouping<int, T>> Split<T>(this IEnumerable<T> elements, int size)
        {
            var list = new List<T>(size);

            var groupCount = 0;
            foreach (T item in elements)
            {
                list.Add(item);
                if (list.Count == size)
                {
                    List<T> chunk = list;
                    list = new List<T>(size);
                    yield return new Group<T>(groupCount++, chunk);
                }
            }

            if (list.Count > 0)
            {
                yield return new Group<T>(groupCount, list);
            }
        }

        private class Group<T> : IGrouping<int, T>
        {
            private int groupCount;
            private IEnumerable<T> enumerable;

            public Group(int groupCount, IEnumerable<T> enumerable)
            {
                this.groupCount = groupCount;
                this.enumerable = enumerable;
            }

            public int Key
            {
                get { return groupCount; }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return enumerable.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return enumerable.GetEnumerator();
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T obj)
        {
            return enumerable.Concat(FromSingle(obj));
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, Maybe<T> maybe)
        {
            return maybe.HasValue ? enumerable.Concat(FromSingle(maybe.Value)) : enumerable;
        }

        private static IEnumerable<T> FromSingle<T>(T obj)
        {
            yield return obj;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, Comparison<T> comparison)
        {
            return enumerable == null ? Enumerable.Empty<T>() : enumerable.OrderBy(t => t, new ComparisonComparer<T>(comparison));
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> collection)
        {
            return collection == null ? new HashSet<T>() : new HashSet<T>(collection);
        }

        public static C Into<T, C>(this IEnumerable<T> enumerable, C collection) where C : ICollection<T>
        {
            foreach (T t in enumerable)
            {
                collection.Add(t);
            }

            return collection;
        }

        /// <summary>
        /// Simply forces EF to load all objects in the IQueryable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        public static void LoadImplicitly<T>(this IQueryable<T> query)
        {
            // the use of for each is no good because maintains the connection open given raise to deadloks
            query.ToList();

        }

        /// <summary>
        /// Iterates the enumerable and performs the given action or each element.
        /// The mane of this extension is compatible with List.ForEach
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null)
            {
                foreach (T t in enumerable)
                {
                    action(t);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable != null)
            {
                int i = 0;
                foreach (T t in enumerable)
                {
                    action(t, i++);
                }
            }
        }

        /// <summary>
        /// Decouples a LINQ to SQL IEnumerable. When converting objects using functions LINQ to SQL does not properly understands and tries 
        /// to convert the function to SQL witch is not possible. An exception occurs. The programmer is forced to execute an equivalent to ToList() before the select in order to
        /// do the Select as in memory operation.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<R> Decouple<T, R>(this IEnumerable<T> enumerable, Expression<Func<IEnumerable<T>, IEnumerable<R>>> expression)
        {
            return expression.Compile()(enumerable.ToList());
        }


        //does not use deferred execution!
        //do not change it!
        public static void ReturnlessZip<T, U>(this IEnumerable<T> firstEnumerable, IEnumerable<U> secondEnumerable, Action<T, U> action)
        {
            using (var firstEnumerator = firstEnumerable.GetEnumerator())
            {
                using (var secondEnumerator = secondEnumerable.GetEnumerator())
                {
                    while (firstEnumerator.MoveNext() && secondEnumerator.MoveNext())
                    {
                        action(firstEnumerator.Current, secondEnumerator.Current);
                    }
                }
            }
        }
    }
}
