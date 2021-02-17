using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Utilities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct ReadOnlyList<T> : IReadOnlyList<T>, IEnumerable<T>, IIndexedEnumerable<T>
    {
        public ReadOnlyList(IList<T>? list)
        {
            this.list = list;
        }

        private readonly IList<T>? list;

        public int Count => (list?.Count).GetValueOrDefault();

        public T this[int index]
        {
            get
            {
                if (list == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return list[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list?.GetEnumerator() ?? EmptyEnumerator();

            static IEnumerator<T> EmptyEnumerator()
            {
                yield break;
            }
        }

        private string GetDebuggerDisplay()
        {
            return $"{typeof(T).Name}[]; Count = {Count}";
        }
    }
}
