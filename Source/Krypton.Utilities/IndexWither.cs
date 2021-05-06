namespace Krypton.Utilities
{
    public readonly struct IndexWither<T>
    {
        public IndexWither(int index, T newValue)
        {
            Index = index;
            NewValue = newValue;
        }

        public int Index { get; }

        public T NewValue { get; }

        public static implicit operator IndexWither<T>((int index, T value) tuple)
            => new(tuple.index, tuple.value);
    }
}