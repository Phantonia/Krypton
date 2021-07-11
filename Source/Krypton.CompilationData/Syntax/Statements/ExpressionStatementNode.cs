using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record ExpressionStatementNode : SingleStatementNode
    {
        public ExpressionStatementNode(ExpressionNode expression,
                                       SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            ExpressionNode = expression;
        }

        public ExpressionNode ExpressionNode { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => ExpressionNode.LexicallyFirstToken;

        public override void WriteCode(TextWriter output)
        {
            ExpressionNode.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
