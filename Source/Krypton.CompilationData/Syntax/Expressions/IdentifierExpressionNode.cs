using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(IdentifierToken identifier,
                                        SyntaxNode? parent = null)
            : base(parent)
        {
            IdentifierToken = identifier;
        }

        public IdentifierToken IdentifierToken { get; }

        public override bool IsLeaf => true;

        public override TypedExpressionNode<IdentifierExpressionNode> Bind(TypeSymbol type)
            => new(this, type);

        public override IdentifierExpressionNode WithParent(SyntaxNode newParent)
            => new(IdentifierToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
