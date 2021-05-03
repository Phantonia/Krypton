using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public sealed class UnaryOperationSymbol : Symbol
    {
        public UnaryOperationSymbol(string name,
                                    Operator @operator,
                                    TypeSymbol operandTypeSymbol,
                                    TypeSymbol returnTypeNode)
            : base(name)
        {
            Debug.Assert(@operator.IsUnary());
            Operator = @operator;
            OperandTypeSymbol = operandTypeSymbol;
            ReturnTypeNode = returnTypeNode;
        }

        public TypeSymbol OperandTypeSymbol { get; }

        public Operator Operator { get; }

        public TypeSymbol ReturnTypeNode { get; }
    }
}
