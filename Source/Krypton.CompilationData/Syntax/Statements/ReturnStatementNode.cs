using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ReturnStatementNode : SingleStatementNode
    {
        public ReturnStatementNode(ReservedKeywordToken returnKeyword,
                                   ExpressionNode? returnedExpression,
                                   SyntaxCharacterToken semicolon,
                                   SyntaxNode? parent = null)
            : base(semicolon, parent)
        {
            ReturnKeywordToken = returnKeyword;
            ReturnedExpressionNode = returnedExpression?.WithParent(this);
        }

        public override bool IsLeaf => ReturnedExpressionNode == null;

        public ExpressionNode? ReturnedExpressionNode { get; }

        public ReservedKeywordToken ReturnKeywordToken { get; }

        protected override string GetDebuggerDisplay()
            => $"{base.GetDebuggerDisplay()}; {(ReturnedExpressionNode == null ? "Without" : "With")} value";

        public ReturnStatementNode WithChildren(ReservedKeywordToken? returnKeyword = null,
                                                ExpressionNode? returnedExpression = null,
                                                // null is valid on ReturnedExpressionNode,
                                                // so do we want to ignore it or set it to null?
                                                // if the param is non-null we always ignore
                                                // overwriteReturnedExpression
                                                bool overwriteReturnedExpression = false,
                                                SyntaxCharacterToken? semicolon = null)
            => new(returnKeyword ?? ReturnKeywordToken,
                   returnedExpression ?? (overwriteReturnedExpression ? null : ReturnedExpressionNode),
                   semicolon ?? SemicolonToken);

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
