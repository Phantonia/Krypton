using Krypton.CompilationData.Symbols;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public abstract class ExpressionNode : SyntaxNode
    {
        private protected ExpressionNode(SyntaxNode? parent) : base(parent) { }

        public bool IsTyped => this is TypedExpressionNode;

        public bool TryGetType([NotNullWhen(true)] out TypeSymbol? type)
        {
            if (this is TypedExpressionNode typedExpression)
            {
                type = typedExpression.TypeSymbol;
                //return true;
            }

            type = null;
            return false;
        }

        public abstract TypedExpressionNode Type(TypeSymbol type);

        public abstract override ExpressionNode WithParent(SyntaxNode newParent);
    }
}
