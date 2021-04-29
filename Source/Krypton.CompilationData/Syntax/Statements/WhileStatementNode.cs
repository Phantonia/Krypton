using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record WhileStatementNode : BodiedStatementNode
    {
        public WhileStatementNode(ReservedKeywordToken whileKeyword,
                                  ExpressionNode condition,
                                  BodyNode body)
            : base(body)
        {
            WhileKeywordToken = whileKeyword;
            ConditionNode = condition;
        }

        public ExpressionNode ConditionNode { get; init; }

        public override bool IsLeaf => false;

        public ReservedKeywordToken WhileKeywordToken { get; init; }

        public override void WriteCode(TextWriter output)
        {
            WhileKeywordToken.WriteCode(output);
            ConditionNode.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}