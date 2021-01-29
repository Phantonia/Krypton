using System.Collections.Generic;

namespace Krypton.Utilities
{
    public static class CollectionHelpers
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

        public static ReadOnlySet<T> MakeReadOnly<T>(this ISet<T>? set)
        {
            return new ReadOnlySet<T>(set);
        }

        public static T? TryGet<T>(this IList<T> list, int index)
            where T : class
        {
            return list.Count > index & index >= 0 ? list[index] : null;
        }
    }
}
