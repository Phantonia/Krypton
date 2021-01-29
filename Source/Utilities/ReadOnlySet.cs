using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Utilities
{
    public readonly struct ReadOnlySet<T> : IReadOnlySet<T>
    {
        public ReadOnlySet(ISet<T>? set)
        {
            this.set = set;
        }

        private readonly ISet<T>? set;

        public int Count => set?.Count ?? 0;

        public bool Contains(T item)
        {
            return set?.Contains(item) ?? false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return set?.SetEquals(other) ?? !other.Any();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return set?.GetEnumerator() ?? EmptyEnumerator();

            static IEnumerator<T> EmptyEnumerator()
            {
                yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool IReadOnlySet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        bool IReadOnlySet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        bool IReadOnlySet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        bool IReadOnlySet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        bool IReadOnlySet<T>.Overlaps(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }
    }
}
