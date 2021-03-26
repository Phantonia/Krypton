using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class LiteralExpressionNode<TLiteral> : ExpressionNode
    {
        public LiteralExpressionNode(LiteralToken<TLiteral> literalToken)
            : this(literalToken, parent: null) { }

        public LiteralExpressionNode(LiteralToken<TLiteral> literalToken, SyntaxNode? parent)
            : base(parent)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            LiteralToken = literalToken;
        }

        public override bool IsLeaf => true;

        public LiteralToken<TLiteral> LiteralToken { get; }

        public override LiteralExpressionNode<TLiteral> WithParent(SyntaxNode newParent)
            => new(LiteralToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            LiteralToken.WriteCode(output);
        }
    }
}
