namespace Krypton.Framework.Symbols
{
    public abstract class ConstantSymbol : NamedFrameworkSymbol
    {
        private protected ConstantSymbol(string name) : base(name) { }
    }

    public sealed class ConstantSymbol<T> : ConstantSymbol
    {
        internal ConstantSymbol(string name, T value) : base(name)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
