using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class WhileStatementNode : BodiedStatementNode
    {
        public WhileStatementNode(ReservedKeywordToken whileKeyword,
                                  ExpressionNode condition,
                                  BodyNode body,
                                  SyntaxNode? parent = null)
            : base(body, parent)
        {
            WhileKeywordToken = whileKeyword;
            ConditionNode = condition.WithParent(this);
        }

        public ExpressionNode ConditionNode { get; }

        public override bool IsLeaf => false;

        public ReservedKeywordToken WhileKeywordToken { get; }

        public WhileStatementNode WithChildren(ReservedKeywordToken? whileKeyword = null,
                                               ExpressionNode? condition = null,
                                               BodyNode? body = null)
            => new(whileKeyword ?? WhileKeywordToken,
                   condition ?? ConditionNode,
                   body ?? BodyNode);

        public override WhileStatementNode WithParent(SyntaxNode newParent)
            => new(WhileKeywordToken, ConditionNode, BodyNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            WhileKeywordToken.WriteCode(output);
            ConditionNode.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}