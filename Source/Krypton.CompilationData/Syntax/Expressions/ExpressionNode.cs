using Krypton.CompilationData.Symbols;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public abstract record ExpressionNode : SyntaxNode
    {
        private protected ExpressionNode() { }

        public bool IsTyped => this is TypedExpressionNode;

        public bool TryGetType([NotNullWhen(true)] out TypeSymbol? type)
        {
            if (this is TypedExpressionNode typedExpression)
            {
                type = typedExpression.TypeSymbol;
                return true;
            }

            type = null;
            return false;
        }

        public TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);
    }
}
