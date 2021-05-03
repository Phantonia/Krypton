namespace Krypton.CompilationData.Symbols
{
    public abstract class ConstantSymbol : Symbol
    {
        private protected ConstantSymbol(string name, TypeSymbol type) : base(name)
        {
            TypeSymbol = type;
        }

        public abstract object ObjectValue { get; }

        public TypeSymbol TypeSymbol { get; }
    }

    public sealed class ConstantSymbol<TValue> : ConstantSymbol
        where TValue : notnull
    {
        public ConstantSymbol(string name, TypeSymbol type, TValue value)
            : base(name, type)
        {
            LiteralHelper.AssertTypeIsLiteralType<TValue>();
            Value = value;
        }

        public TValue Value { get; }

        public override object ObjectValue => Value;
    }
}
