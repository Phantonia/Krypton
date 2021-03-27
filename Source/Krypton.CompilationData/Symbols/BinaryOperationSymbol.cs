using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public sealed class BinaryOperationSymbol : Symbol
    {
        public BinaryOperationSymbol(string name,
                                     TypeSymbol leftOperandTypeSymbol,
                                     Operator @operator,
                                     TypeSymbol rightOperandTypeSymbol,
                                     TypeSymbol returnTypeSymbol)
            : base(name)
        {
            Debug.Assert(@operator.IsBinary());
            LeftOperandTypeSymbol = leftOperandTypeSymbol;
            Operator = @operator;
            RightOperandTypeSymbol = rightOperandTypeSymbol;
            ReturnTypeSymbol = returnTypeSymbol;
        }

        public TypeSymbol LeftOperandTypeSymbol { get; }

        public Operator Operator { get; }

        public TypeSymbol ReturnTypeSymbol { get; }

        public TypeSymbol RightOperandTypeSymbol { get; }
    }
}
