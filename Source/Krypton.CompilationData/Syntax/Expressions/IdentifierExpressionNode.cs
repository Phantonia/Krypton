using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(IdentifierToken identifierToken)
            : this(identifierToken, parent: null) { }

        public IdentifierExpressionNode(IdentifierToken identifierToken, SyntaxNode? parent)
            : base(parent)
        {
            IdentifierToken = identifierToken;
        }

        public IdentifierToken IdentifierToken { get; }

        public override bool IsLeaf => true;

        public override IdentifierExpressionNode WithParent(SyntaxNode newParent)
            => new(IdentifierToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
