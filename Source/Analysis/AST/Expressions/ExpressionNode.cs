using Krypton.Analysis.Ast.Symbols;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Expressions
{
    public abstract class ExpressionNode : Node
    {
        private protected ExpressionNode(int lineNumber, int index) : base(lineNumber, index) { }

        public ImplicitConversionSymbolNode? ImplicitConversionNodeIfAny { get; private set; }

        public void SpecifyImplicitConversion(ImplicitConversionSymbolNode conversion)
        {
            Debug.Assert(ImplicitConversionNodeIfAny == null);

            ImplicitConversionNodeIfAny = conversion;
        }
    }
}
