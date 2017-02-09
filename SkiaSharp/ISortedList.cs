using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Svg.Interfaces
{
    public interface ISortedList<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IList<TValue> Values { get; }
        IList<TKey> Keys { get; }
    }
}
