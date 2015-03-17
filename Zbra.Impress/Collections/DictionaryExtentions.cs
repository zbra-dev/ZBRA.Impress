
using System;
using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public static class DictionaryExtentions
    {
        /// <summary>
        /// Returns an immutable IDictionary with zero elements. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>An immutable IDictionary with zero elements. </returns>
        public static IDictionary<K, V> EmptyDictionary<K, V>()
        {
            return new Dictionary<K, V>(0);
        }

        public static IDictionary<K, V> OrEmpty<K, V>(this Maybe<IDictionary<K, V>> maybe)
        {
            return maybe.Or(EmptyDictionary<K, V>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>true if the element is added by the first time, false otherwise</returns>
        public static bool ReplaceOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }
        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            V value;
            if (dictionary.Count != 0 && dictionary.TryGetValue(key, out value))
            {
                return value.ToMaybe();
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, K key)
        {
            Maybe<V> value;
            if (dictionary.Count != 0 && dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key)
        {
            if (key.HasValue)
            {
                Maybe<V> value;
                if (dictionary.Count != 0 && dictionary.TryGetValue(key.Value, out value))
                {
                    return value;
                }
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGet<K, V>(this IDictionary<K, V> dictionary, Maybe<K> key)
        {
            if (key.HasValue)
            {
                V value;
                if (dictionary.Count != 0 && dictionary.TryGetValue(key.Value, out value))
                {
                    return value.ToMaybe();
                }
            }

            return Maybe<V>.Nothing;
        }


        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, V> dictionary, K key, Func<K, V> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value.ToMaybe();
            }
            else
            {
                value = constructor(key);
                dictionary.Add(key, value);
                return value.ToMaybe();
            }

        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, Maybe<V>> dictionary, K key, Func<K, Maybe<V>> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            Maybe<V> value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                value = constructor(key);
                dictionary.Add(key, value);
                return value;
            }
        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, Maybe<V>> dictionary, Maybe<K> key, Func<K, Maybe<V>> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            if (key.HasValue)
            {
                Maybe<V> value;
                if (dictionary.TryGetValue(key.Value, out value))
                {
                    return value;
                }
                else
                {
                    value = constructor(key.Value);
                    dictionary.Add(key.Value, value);
                    return value;
                }
            }
            return Maybe<V>.Nothing;
        }

        public static Maybe<V> MaybeGetOrAdd<K, V>(this IDictionary<K, V> dictionary, Maybe<K> key, Func<K, V> constructor)
        {
            if (dictionary == null)
            {
                return Maybe<V>.Nothing;
            }

            if (key.HasValue)
            {
                V value;
                if (dictionary.TryGetValue(key.Value, out value))
                {
                    return value.ToMaybe();
                }
                else
                {
                    value = constructor(key.Value);
                    dictionary.Add(key.Value, value);
                    return value.ToMaybe();
                }
            }

            return Maybe<V>.Nothing;
        }
    }
}
