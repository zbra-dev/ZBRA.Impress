using System.Collections.Generic;

namespace Zbra.Impress.Collections
{
    public interface IReadOnlyMultiSet<T> : IEnumerable<KeyValuePair<T, int>>
    {
        int Count { get; }
        ISet<T> Values { get; }
        bool IsReadOnly();
    }
}
