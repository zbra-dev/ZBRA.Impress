
namespace ZBRA.Impress.Collections
{
    public interface IMultiSet<T> : IReadOnlyMultiSet<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns> true is the value is new in the multiset, false otherwise</returns>
        bool Add(T value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="occurrences"></param>
        /// <returns>the ocurrences in the multiset previous to add</returns>
        int Add(T value, int occurrences);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true is the values was removed, false otherwise. Is false if the value was not in the multiset</returns>
        bool Remove(T value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="occurences"></param>
        /// <returns>the ocurrences in the multiset previous to remove</returns>
        int Remove(T value, int occurences);
        int GetOccurencesCount(T value);
        void Clear();
        int Clear(T value);
        bool Contains(T value);

        void AddAll(IMultiSet<T> other);
    }
}
