using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework
{
    internal static class CollectionHelpers
    {
        public static ReadOnlyList<T> MakeReadOnly<T>(this IList<T>? list)
            where T : class
        {
            return new ReadOnlyList<T>(list);
        }

        public static ReadOnlyDictionary<TKey, TValue> MakeReadOnly<TKey, TValue>(this IDictionary<TKey, TValue>? dict)
            where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }
    }
}
