using System.Collections;
using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public class DictionaryMultiSet<T> : IMultiSet<T>
    {
        public int Count { get; private set; }
        public ISet<T> Values { get { return new HashSet<T>(valuesToCountMapping.Keys); } }

        private IDictionary<T, int> valuesToCountMapping;

        public DictionaryMultiSet()
        {
            valuesToCountMapping = new Dictionary<T, int>();
        }

        public bool Add(T value)
        {
            Count++;
            if (valuesToCountMapping.ContainsKey(value))
            {
                valuesToCountMapping[value]++;
                return false;
            }
            else
            {
                valuesToCountMapping.Add(value, 1);
                return true;
            }
        }

        public int Add(T value, int occurrences)
        {
            Count += occurrences;

            int count = 0;
            if (valuesToCountMapping.TryGetValue(value, out count))
            {
                valuesToCountMapping[value] = count + occurrences;
            }
            else
            {
                valuesToCountMapping.Add(value, occurrences);
            }

            return count;
        }

        public bool Remove(T value)
        {
            if (valuesToCountMapping.ContainsKey(value))
            {
                var success = true;
                if (valuesToCountMapping[value] == 1)
                {
                    success = valuesToCountMapping.Remove(value);
                }
                else
                {
                    valuesToCountMapping[value]--;
                }
                Count--;

                return success;
            }
            return false;
        }

        public int Remove(T value, int occurences)
        {
            int count = 0;
            if (valuesToCountMapping.TryGetValue(value, out count))
            {
                if (count <= occurences)
                {
                    valuesToCountMapping.Remove(value);
                    Count -= count;
                }
                else
                {
                    valuesToCountMapping[value] -= occurences;
                    Count -= occurences;
                }

            }
            return count;
        }

        public int GetOccurencesCount(T value)
        {
            int count = 0;
            valuesToCountMapping.TryGetValue(value, out count);
            return count;
        }

        public void Clear()
        {
            valuesToCountMapping.Clear();
            Count = 0;
        }

        public int Clear(T value)
        {

            int count = 0;
            if (valuesToCountMapping.TryGetValue(value, out count))
            {
                valuesToCountMapping.Remove(value);
                Count -= count;
            }
            return count;
        }

        public bool Contains(T item)
        {
            return valuesToCountMapping.ContainsKey(item);
        }

        public bool IsReadOnly()
        {
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return valuesToCountMapping.GetEnumerator();
        }

        IEnumerator<KeyValuePair<T, int>> IEnumerable<KeyValuePair<T, int>>.GetEnumerator()
        {
            return valuesToCountMapping.GetEnumerator();
        }

        public void AddAll(IMultiSet<T> other)
        {
            foreach (var keyValuePair in other)
            {
                Add(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
