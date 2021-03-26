using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ReturnStatementNode : StatementNode
    {
        public ReturnStatementNode(ReservedKeywordToken returnKeywordToken,
                                   ExpressionNode? returnedExpressionNode,
                                   SyntaxCharacterToken semicolonToken,
                                   SyntaxNode? parent = null)
            : base(parent)
        {
            ReturnKeywordToken = returnKeywordToken;
            ReturnedExpressionNode = returnedExpressionNode?.WithParent(this);
            SemicolonToken = semicolonToken;
        }

        public override bool IsLeaf => ReturnedExpressionNode == null;

        public ExpressionNode? ReturnedExpressionNode { get; }

        public ReservedKeywordToken ReturnKeywordToken { get; }

        public SyntaxCharacterToken SemicolonToken { get; }

        public override ReturnStatementNode WithParent(SyntaxNode newParent)
            => new(ReturnKeywordToken, ReturnedExpressionNode, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            ReturnKeywordToken.WriteCode(output);
            ReturnedExpressionNode?.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
