using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Krypton.Utilities
{
    public static class CollectionHelpers
    {
        public static bool IsSingle<T>(this IEnumerable<T> source, [NotNullWhen(true)] out T? single)
            where T : notnull
        {
            if (source is IList<T> list)
            {
                if (list.Count == 1)
                {
                    single = list[0];
                    return true;
                }
            }
            else
            {
                using IEnumerator<T> enumerator = source.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    single = enumerator.Current;

                    if (!enumerator.MoveNext())
                    {
                        return true;
                    }
                }
            }

            single = default;
            return false;
        }

        public static ReadOnlyList<T> MakeReadOnly<T>(this IEnumerable<T>? enumerable)
            where T : class
        {
            if (enumerable == null)
            {
                return default;
            }

            if (enumerable is ReadOnlyList<T> readOnlyList)
            {
                return readOnlyList;
            }

            IList<T> list = (enumerable as IList<T>) ?? enumerable.ToList();
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
