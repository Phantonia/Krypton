using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record ReturnStatementNode : SingleStatementNode
    {
        public ReturnStatementNode(ReservedKeywordToken returnKeyword,
                                   ExpressionNode? returnedExpression,
                                   SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            ReturnKeywordToken = returnKeyword;
            ReturnedExpressionNode = returnedExpression;
        }

        public override bool IsLeaf => ReturnedExpressionNode == null;

        public override Token LexicallyFirstToken => ReturnKeywordToken;

        public ExpressionNode? ReturnedExpressionNode { get; init; }

        public ReservedKeywordToken ReturnKeywordToken { get; init; }

        protected override string GetDebuggerDisplay()
            => $"{base.GetDebuggerDisplay()}; {(ReturnedExpressionNode == null ? "Without" : "With")} value";

        public override void WriteCode(TextWriter output)
        {
            ReturnKeywordToken.WriteCode(output);
            ReturnedExpressionNode?.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
