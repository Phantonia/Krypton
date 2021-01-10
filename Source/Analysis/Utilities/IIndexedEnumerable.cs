using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.Utilities
{
    public interface IIndexedEnumerable<T> : IEnumerable<T>
    {
        T this[int index] { get; }

        int Count { get; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public readonly struct Success
    {
        public Success(bool value)
        {
            this.value = value;
        }

        private readonly bool value;

        public static implicit operator bool(Success s) => s.value;
        public static implicit operator Success(bool b) => new(b);

        public static Success operator &(Success x, Success y) => new(x.value & y.value);
        public static Success operator |(Success x, Success y) => new(x.value | y.value);
        public static Success operator ^(Success x, Success y) => new(x.value ^ y.value);

        public static bool operator true(Success s) => s.value;
        public static bool operator false(Success s) => !s.value;
    }
}
