namespace Krypton.CompilationData.Symbols
{
    public abstract class ConstantSymbol : Symbol
    {
        private protected ConstantSymbol(string name) : base(name) { }

        public abstract object ValueAsObject { get; }
    }

    public sealed class ConstantSymbol<TValue> : ConstantSymbol
        where TValue : notnull
    {
        public ConstantSymbol(string name, TValue value) : base(name)
        {
            LiteralHelper.AssertTypeIsLiteralType<TValue>();
            Value = value;
        }

        public TValue Value { get; }

        public override object ValueAsObject => Value;
    }
}
