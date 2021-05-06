using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Krypton.Utilities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct FinalDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEquatable<FinalDictionary<TKey, TValue>>
        where TKey : notnull
    {
        public FinalDictionary(IDictionary<TKey, TValue>? dict)
        {
            this.dict = dict;
        }

        private readonly IDictionary<TKey, TValue>? dict;

        public TValue this[TKey key]
            => dict != null ? dict[key] : throw new KeyNotFoundException();

        public IEnumerable<TKey> Keys
            => dict?.Keys ?? Array.Empty<TKey>();

        public IEnumerable<TValue> Values
            => dict?.Values ?? Array.Empty<TValue>();

        public int Count => dict?.Count ?? 0;

        public bool ContainsKey(TKey key)
        {
            return dict?.ContainsKey(key) ?? false;
        }

        public override bool Equals(object? obj)
            => obj is FinalDictionary<TKey, TValue> dictionary && Equals(dictionary);

        public bool Equals(FinalDictionary<TKey, TValue> other)
            => Count == other.Count
            && !this.Except(other, new KeyValuePairEqualityComparer()).Any();

        private string GetDebuggerDisplay()
        {
            return $"[{typeof(TKey).Name} = {typeof(TValue).Name}]; Count = {Count}";
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict?.GetEnumerator() ?? EmptyEnumerator();

            static IEnumerator<KeyValuePair<TKey, TValue>> EmptyEnumerator()
            {
                yield break;
            }
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();

            foreach (var kvp in this)
            {
                hashCode.Add(kvp);
            }

            return hashCode.ToHashCode();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
            return dict?.TryGetValue(key, out value) ?? false;
        }

        public static bool operator ==(FinalDictionary<TKey, TValue> left, FinalDictionary<TKey, TValue> right)
            => left.Equals(right);

        public static bool operator !=(FinalDictionary<TKey, TValue> left, FinalDictionary<TKey, TValue> right)
            => !(left == right);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct KeyValuePairEqualityComparer : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
                => EqualityComparer<TKey>.Default.Equals(x.Key, y.Key)
                && EqualityComparer<TValue>.Default.Equals(x.Value, y.Value);

            public int GetHashCode([DisallowNull] KeyValuePair<TKey, TValue> obj)
                => HashCode.Combine(obj.Key, obj.Value);
        }
    }
}