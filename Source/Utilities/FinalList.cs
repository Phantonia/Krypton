using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Utilities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct FinalList<T> : IReadOnlyList<T>, IEnumerable<T>, IIndexedEnumerable<T>, IEquatable<FinalList<T>>
    {
        public FinalList(IList<T>? list)
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
            return $"[ {string.Join(", ", this)} ]";
        }

        public override bool Equals(object? obj)
            => obj is FinalList<T> list && Equals(list);

        public bool Equals(FinalList<T> other)
            => this.SequenceEqual(other);

        public override int GetHashCode()
        {
            HashCode hashCode = new();

            foreach (T item in this)
            {
                hashCode.Add(item);
            }

            return hashCode.ToHashCode();
        }

        public T? TryGet(int index)
            => index < Count && index >= 0
            ? this[index]
            : default;

        public static bool operator ==(FinalList<T> left, FinalList<T> right) => left.Equals(right);

        public static bool operator !=(FinalList<T> left, FinalList<T> right) => !(left == right);
    }
}
