using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ExpressionStatementNode : StatementNode
    {
        public ExpressionStatementNode(ExpressionNode expressionNode,
                                       SyntaxCharacterToken semicolonToken,
                                       SyntaxNode? parent = null)
            : base(parent)
        {
            ExpressionNode = expressionNode.WithParent(this);
            SemicolonToken = semicolonToken;
        }

        public ExpressionNode ExpressionNode { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken SemicolonToken { get; }

        public ExpressionStatementNode WithChildren(ExpressionNode? expressionNode = null,
                                                    SyntaxCharacterToken? semicolonToken = null)
            => new(expressionNode ?? ExpressionNode,
                   semicolonToken ?? SemicolonToken);

        public override ExpressionStatementNode WithParent(SyntaxNode newParent)
            => new(ExpressionNode, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            ExpressionNode.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
