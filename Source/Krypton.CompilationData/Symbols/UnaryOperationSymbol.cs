using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public abstract class UnaryOperationSymbol : Symbol
    {
        private protected UnaryOperationSymbol(string name,
                                               Operator @operator)
            : base(name)
        {
            Debug.Assert(@operator.IsUnary());
            Operator = @operator;
        }

        public abstract TypeSymbol OperandTypeSymbol { get; }

        public Operator Operator { get; }

        public abstract TypeSymbol ReturnTypeSymbol { get; }
    }
}
