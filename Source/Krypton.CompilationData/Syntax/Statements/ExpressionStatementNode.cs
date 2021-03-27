using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ExpressionStatementNode : SingleStatementNode
    {
        public ExpressionStatementNode(ExpressionNode expression,
                                       SyntaxCharacterToken semicolon,
                                       SyntaxNode? parent = null)
            : base(semicolon, parent)
        {
            ExpressionNode = expression.WithParent(this);
        }

        public ExpressionNode ExpressionNode { get; }

        public override bool IsLeaf => false;

        public ExpressionStatementNode WithChildren(ExpressionNode? expression = null,
                                                    SyntaxCharacterToken? semicolon = null)
            => new(expression ?? ExpressionNode,
                   semicolon ?? SemicolonToken);

        public override ExpressionStatementNode WithParent(SyntaxNode newParent)
            => new(ExpressionNode, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            ExpressionNode.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
