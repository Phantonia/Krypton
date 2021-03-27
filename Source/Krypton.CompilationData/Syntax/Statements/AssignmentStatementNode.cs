using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class AssignmentStatementNode : SingleStatementNode
    {
        public AssignmentStatementNode(ExpressionNode assignedExpression,
                                       SyntaxCharacterToken equals,
                                       ExpressionNode newValue,
                                       SyntaxCharacterToken semicolon,
                                       SyntaxNode? parent = null)
            : base(semicolon, parent)
        {
            AssignedExpressionNode = assignedExpression.WithParent(this);
            EqualsToken = equals;
            NewValueNode = newValue.WithParent(this);

            Debug.Assert(EqualsToken.SyntaxCharacter == SyntaxCharacter.Equals);
        }

        public ExpressionNode AssignedExpressionNode { get; }
        
        public SyntaxCharacterToken EqualsToken { get; }

        public override bool IsLeaf => false;

        public ExpressionNode NewValueNode { get; }

        public AssignmentStatementNode WithChildren(ExpressionNode? assignedExpression = null,
                                                    SyntaxCharacterToken? equals = null,
                                                    ExpressionNode? newValue = null,
                                                    SyntaxCharacterToken? semicolon = null)
            => new(assignedExpression ?? AssignedExpressionNode,
                   equals ?? EqualsToken,
                   newValue ?? NewValueNode,
                   semicolon ?? SemicolonToken);

        public override AssignmentStatementNode WithParent(SyntaxNode newParent)
            => new(AssignedExpressionNode, EqualsToken, NewValueNode, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            AssignedExpressionNode.WriteCode(output);
            EqualsToken.WriteCode(output);
            NewValueNode.WriteCode(output);
        }
    }
}
