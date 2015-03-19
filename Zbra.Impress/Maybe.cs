using System;
using System.Collections.Generic;
using System.Reflection;


namespace ZBRA.Impress
{

    public struct Maybe<T>
    {

        public readonly static Maybe<T> Nothing = new Maybe<T>(false);

        public static Maybe<X> ValueOf<X>(X value) where X : class
        {
            return value == null ? Maybe<X>.Nothing : new Maybe<X>(value);
        }

        private static Maybe<X> ValueOfValue<X>(X value)
        {
            return value == null ? Maybe<X>.Nothing : new Maybe<X>(value);
        }

        public static Maybe<X> ValueOfStruct<X>(X value) where X : struct
        {
            return new Maybe<X>(value);
        }

        public static Maybe<X> ValueOfNothing<X>()
        {
            return Maybe<X>.Nothing;
        }


        private T obj;
        private bool hasValue;

        private Maybe(bool hasValue)
        {
            this.hasValue = hasValue;
            this.obj = default(T);
        }

        internal Maybe(T obj)
        {
            this.obj = obj;
            hasValue = true;
        }

        public bool HasValue { get { return hasValue; } }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new Exception("No Value is present");
                }

                return obj;
            }
        }

        public T Or(T defaultValue)
        {
            return hasValue ? obj : defaultValue;
        }

        public bool Equals(Maybe<T> other)
        {
            if (this.hasValue)
            {
                return other.HasValue && this.Value.Equals(other.Value);
            }
            else
            {
                return !other.HasValue;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
            {
                return false;
            }
            return Equals((Maybe<T>)obj);
        }

        public override int GetHashCode()
        {
            return this.hasValue ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return this.HasValue ? Value.ToString() : string.Empty;
        }

        public bool Is(T other)
        {
            return this.HasValue && this.Value.Equals(other);
        }

        public bool Is(Func<T, bool> predicate)
        {
            return this.HasValue && predicate(this.Value);
        }

        public Maybe<T> AlsoNothing(T value)
        {
            if (this.Is(value))
            {
                return Maybe<T>.Nothing;
            }
            return this;
        }

        public Maybe<T> AlsoNothing(Func<T, bool> predicate)
        {
            if (this.Is(predicate))
            {
                return Maybe<T>.Nothing;
            }
            return this;
        }

        [Obsolete]
        public Maybe<T> ToMaybe()
        {
            return this;
        }
    }

    public static class MaybeMonadExtention
    {
        public static Nullable<T> ToNullable<T>(this Maybe<T> value) where T : struct
        {
            if (!value.HasValue)
            {
                return null;
            }
            else
            {
                return value.Value;
            }
        }

        public static Maybe<T> Convert<T>(this Maybe<string> value) where T : struct
        {
            if (!value.HasValue)
            {
                return Maybe<T>.Nothing;
            }
            else
            {
                try
                {
                    return GenericTypeConverter.GetConverter().Convert<T>(value.Value).ToMaybe();
                }
                catch (ConverterException)
                {
                    return Maybe<T>.Nothing;
                }
            }
        }

        public static S Or<S>(this Nullable<S> value, S defaultValue) where S : struct
        {
            return value.HasValue ? value.Value : defaultValue;
        }

        public static Nullable<S> AlsoNothing<S>(this Nullable<S> nullable, S value) where S : struct
        {
            if (!nullable.HasValue || nullable.Value.Equals(value))
            {
                return (Nullable<S>)null;
            }
            return nullable;
        }

        public static Nullable<S> AlsoNothing<S>(this Nullable<S> nullable, Func<S, bool> predicate) where S : struct
        {
            if (!nullable.HasValue || predicate(nullable.Value))
            {
                return (Nullable<S>)null;
            }
            return nullable;
        }

        public static Maybe<V> Select<S, V>(this Nullable<S> m, Func<S, Maybe<V>> k)
            where S : struct
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value);

        }
        public static Nullable<V> Select<S, V>(this Nullable<S> m, Func<S, V> k)
            where S : struct
            where V : struct
        {
            return !m.HasValue ? (Nullable<V>)null : k(m.Value);
        }

        public static Nullable<V> Select<S, V>(this Nullable<S> m, Func<S, Nullable<V>> k)
            where S : struct
            where V : struct
        {
            return !m.HasValue ? (Nullable<V>)null : k(m.Value);
        }

        public static Maybe<S> ToMaybe<S>(this Nullable<S> value) where S : struct
        {
            return !value.HasValue ? Maybe<S>.Nothing : new Maybe<S>(value.Value);
        }

        public static Maybe<string> ToMaybe(this string value)
        {
            return string.IsNullOrEmpty(value) ? Maybe<string>.Nothing : new Maybe<string>(value);
        }

        public static Maybe<bool> Negate(this Maybe<bool> value)
        {
            return value.Select(it => !it);
        }

        public static string OrEmpty(this Maybe<string> value)
        {
            return value.Or(string.Empty);
        }

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            if (value == null)
            {
                return Maybe<T>.Nothing;
            }

            // treat the case where value is already a Maybe. 
            // not necessarily a Maybe of T. Assume is of type R where R is subtype of T
            // if its not return nothing
            var valueType = value.GetType();

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Maybe<>))
            {
                bool hasValue = (bool)valueType.GetProperty("HasValue").GetGetMethod().Invoke(value, null);

                if (hasValue)
                {
                    var val = valueType.GetProperty("Value").GetGetMethod().Invoke(value, null);
                    if (val is T)
                    {
                        return new Maybe<T>((T)val);
                    }
                }
                return Maybe<T>.Nothing;
            }
            return new Maybe<T>((T)value);
        }

        public static Maybe<TTarget> MaybeCast<TOrigin, TTarget>(this TOrigin value)
        {
            try
            {
                object v = value;
                return value == null ? Maybe<TTarget>.Nothing : ((TTarget)v).ToMaybe();
            }
            catch (InvalidCastException)
            {
                return Maybe<TTarget>.Nothing;
            }
        }

        public static Maybe<TTarget> MaybeCast<TOrigin, TTarget>(this Maybe<TOrigin> value)
        {
            try
            {
                return !value.HasValue ? Maybe<TTarget>.Nothing : ((TTarget)(object)value.Value).ToMaybe();
            }
            catch (InvalidCastException)
            {
                return Maybe<TTarget>.Nothing;
            }
        }

        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, V> k)
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value).ToMaybe();
        }

        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, Maybe<V>> k)
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value);
        }

        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, Nullable<V>> k) where V : struct
        {
            return !m.HasValue ? Maybe<V>.Nothing : ToMaybe(k(m.Value));
        }

        public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> m, Func<T, Maybe<U>> k, Func<T, U, V> s)
        {
            return m.SelectMany(x => k(x).SelectMany(y => s(x, y).ToMaybe()));
        }

        public static Maybe<V> SelectMany<T, V>(this Maybe<T> m, Func<T, Maybe<V>> k)
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value);
        }

        /// <summary>
        /// Compares two Maybe objects that augment a type that implements the IComparable interface.
        /// CompareTo considers that Maybe.Nothing is less than every other value of Maybe.
        /// </summary>
        /// <typeparam name="T">A type that implements IComparable</typeparam>
        /// <param name="x">The value to compare</param>
        /// <param name="y">The value to compare with</param>
        /// <returns>0 if the values are the same, or both are Nothing. 1 if x is larger than y, or y is Nothing, and -1 if y is larger than x or x is Nothing.</returns>
        public static int CompareTo<T>(this Maybe<T> x, Maybe<T> y) where T : IComparable<T>
        {
            if (x.HasValue && y.HasValue)
            {
                return x.Value.CompareTo(y.Value);
            }
            else if (!x.HasValue && !y.HasValue)
            {
                return 0;
            }
            else if (x.HasValue)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Compares two Maybe objects using a given implementation of an IComparer
        /// CompareTo considers that Maybe.Nothing is less than every other value of Maybe.
        /// </summary>
        /// <typeparam name="T">A type that implements IComparable</typeparam>
        /// <param name="x">The value to compare</param>
        /// <param name="y">The value to compare with</param>
        /// <param name="comparer">The IComparer to use in the comparation</param>
        /// <returns>0 if the values are the same, or both are Nothing. 1 if x is larger than y, or y is Nothing, and -1 if y is larger than x or x is Nothing.</returns>
        public static int CompareTo<T>(this Maybe<T> x, Maybe<T> y, IComparer<T> comparer)
        {
            if (x.HasValue && y.HasValue)
            {
                return comparer.Compare(x.Value, y.Value);
            }
            else if (!x.HasValue && !y.HasValue)
            {
                return 0;
            }
            else if (x.HasValue)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        public static IEnumerable<T> Existing<T>(this IEnumerable<Maybe<T>> all)
        {
            foreach (Maybe<T> t in all)
            {
                if (t.HasValue)
                {
                    yield return t.Value;
                }
            }
        }

        public static IEnumerable<S> Existing<S>(this IEnumerable<Nullable<S>> all) where S : struct
        {
            foreach (Nullable<S> s in all)
            {
                if (s.HasValue)
                {
                    yield return s.Value;
                }
            }
        }

        public static Maybe<T> WithAlternative<T>(this Maybe<T> m, T alternative)
        {
            if (!m.HasValue)
            {
                return alternative.ToMaybe();
            }
            return m;
        }

        public static Maybe<T> WithAlternative<T>(this Maybe<T> m, Maybe<T> alternative)
        {
            if (!m.HasValue)
            {
                return alternative;
            }
            return m;
        }

        public static Maybe<E> MaybeEnum<E>(this Maybe<int> m) where E : struct
        {
            if (!typeof(E).IsEnum)
            {
                return Maybe<E>.Nothing;
            }

            try
            {
                return m.Select(v => v.ToEnum<E>());
            }
            catch (EnumConversionException)
            {
                return Maybe<E>.Nothing;
            }
        }

        public static Maybe<E> MaybeEnum<E>(this Maybe<string> m) where E : struct
        {
            if (typeof(E).IsEnum && m.HasValue)
            {
                E value;
                if (Enum.TryParse(m.Value, out value))
                {
                    return value.ToMaybe();
                }
            }

            return Maybe<E>.Nothing;
        }
    }


    public static class MaybeReflection
    {
        public static object ReflectionMaybeNothing(Type returnValueType)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfNothing").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, null);
        }

        public static object ReflectionMaybeStruct(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfStruct").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }


        public static object ReflectionMaybe(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfValue", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }

    }

}
