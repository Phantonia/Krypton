using Krypton.CompilationData.Syntax.Declarations;

namespace Krypton.CompilationData.Symbols
{
    public abstract class ConstantSymbol : Symbol
    {
        private protected ConstantSymbol(string name) : base(name) { }

        public abstract object ObjectValue { get; }

        public abstract TypeSymbol TypeSymbol { get; }
    }

    public abstract class ConstantSymbol<TValue> : ConstantSymbol
        where TValue : notnull
    {
        public ConstantSymbol(string name, TValue value)
            : base(name)
        {
            LiteralHelper.AssertTypeIsLiteralType<TValue>();
            Value = value;
        }

        public TValue Value { get; }

        public override object ObjectValue => Value;
    }

    public sealed class InternalConstantSymbol<TValue> : ConstantSymbol<TValue>, IInternalSymbol
        where TValue : notnull
    {
        public InternalConstantSymbol(ConstantDeclarationNode constant,
                                      TypeSymbol type,
                                      TValue value)
            : base(new string(constant.Name.Span), value)
        {
            ConstantDeclarationNode = constant;
            TypeSymbol = type;
        }

        public ConstantDeclarationNode ConstantDeclarationNode { get; }

        public override TypeSymbol TypeSymbol { get; }

        DeclarationNode IInternalSymbol.DeclarationNode => ConstantDeclarationNode;
    }
}
