namespace Krypton.Framework.Symbols
{
    public sealed class UnaryOperationSymbol : FrameworkSymbol
    {
        internal UnaryOperationSymbol(Operator @operator, FrameworkType operandType, FrameworkType returnType, UnaryGenerator generator)
        {
            Operator = @operator;
            OperandType = operandType;
            ReturnType = returnType;
            Generator = generator;
        }

        public UnaryGenerator Generator { get; }

        public FrameworkType OperandType { get; }

        public Operator Operator { get; }

        public FrameworkType ReturnType { get; }
    }
}
