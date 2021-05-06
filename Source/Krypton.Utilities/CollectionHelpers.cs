using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Krypton.Utilities
{
    public static class CollectionHelpers
    {
        public static IEnumerable<T> AddItem<T>(this IEnumerable<T> source, T item)
        {
            foreach (T i in source)
            {
                yield return i;
            }

            yield return item;
        }

        public static IEnumerable<T> TakeAllButLast<T>(this IEnumerable<T> source)
        {
            using IEnumerator<T> enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                yield break;
            }

            while (true)
            {
                T item = enumerator.Current;

                if (enumerator.MoveNext())
                {
                    yield return item;
                }
            }
        }

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

        public static FinalList<T> Finalize<T>(this IEnumerable<T>? enumerable)
            where T : class
        {
            if (enumerable == null)
            {
                return default;
            }

            if (enumerable is FinalList<T> readOnlyList)
            {
                return readOnlyList;
            }

            IList<T> list = (enumerable as IList<T>) ?? enumerable.ToList();
            return new FinalList<T>(list);
        }

        public static FinalDictionary<TKey, TValue> Finalize<TKey, TValue>(this IDictionary<TKey, TValue>? dict)
            where TKey : notnull
        {
            return new FinalDictionary<TKey, TValue>(dict);
        }

        public static T? TryGet<T>(this IList<T> list, int index)
            where T : class
        {
            return list.Count > index & index >= 0 ? list[index] : null;
        }

        public static FinalList<T> With<T>(this FinalList<T> original, params IndexWither<T>[]? withers)
            where T : class
        {
            if ((withers?.Length ?? 0) == 0)
            {
                // withers is either empty or null
                return original;
            }

            List<T> list = new(original.Count);

            Dictionary<int, T> hashedWithers = withers!.ToDictionary(w => w.Index, w => w.NewValue);

            for (int i = 0; i < original.Count; i++)
            {
                if (hashedWithers.TryGetValue(i, out T? newValue))
                {
                    list.Add(newValue);
                }
                else
                {
                    list.Add(original[i]);
                }
            }

            return list.Finalize();
        }
    }
}