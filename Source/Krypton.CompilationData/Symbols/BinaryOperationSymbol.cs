using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public abstract class BinaryOperationSymbol : Symbol
    {
        private protected BinaryOperationSymbol(string name,
                                                Operator @operator)
            : base(name)
        {
            Debug.Assert(@operator.IsBinary());
            Operator = @operator;
        }

        public abstract TypeSymbol LeftOperandTypeSymbol { get; }

        public Operator Operator { get; }

        public abstract TypeSymbol ReturnTypeSymbol { get; }

        public abstract TypeSymbol RightOperandTypeSymbol { get; }
    }
}
