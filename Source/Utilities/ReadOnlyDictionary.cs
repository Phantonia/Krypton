using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Krypton.Utilities
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ReadOnlyDictionary(IDictionary<TKey, TValue>? dict)
        {
            this.dict = dict!;
        }

        private readonly IDictionary<TKey, TValue>? dict;

        public TValue this[TKey key] => dict != null ? dict[key] : throw new KeyNotFoundException();

        public IEnumerable<TKey> Keys => dict?.Keys ?? Enumerable.Empty<TKey>();

        public IEnumerable<TValue> Values => dict?.Values! ?? Enumerable.Empty<TValue>();

        public int Count => dict?.Count ?? 0;

        public bool ContainsKey(TKey key)
        {
            return dict?.ContainsKey(key) ?? false;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            value = default;
#nullable disable
            return dict?.TryGetValue(key, out value) ?? false;
#nullable restore
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict?.GetEnumerator() ?? EmptyEnumerator();

            static IEnumerator<KeyValuePair<TKey, TValue>> EmptyEnumerator()
            {
                yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GetDebuggerDisplay()
        {
            return $"[{typeof(TKey).Name} = {typeof(TValue).Name}]; Count = {Count}";
        }
    }
}
