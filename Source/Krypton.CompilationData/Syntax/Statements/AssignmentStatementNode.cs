using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record AssignmentStatementNode : SingleStatementNode
    {
        public AssignmentStatementNode(ExpressionNode assignedExpression,
                                       SyntaxCharacterToken equals,
                                       ExpressionNode newValue,
                                       SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            AssignedExpressionNode = assignedExpression;
            EqualsToken = equals;
            NewValueNode = newValue;

            Debug.Assert(EqualsToken.SyntaxCharacter == SyntaxCharacter.Equals);
        }

        public ExpressionNode AssignedExpressionNode { get; init; }
        
        public SyntaxCharacterToken EqualsToken { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => AssignedExpressionNode.LexicallyFirstToken;

        public ExpressionNode NewValueNode { get; init; }

        public override void WriteCode(TextWriter output)
        {
            AssignedExpressionNode.WriteCode(output);
            EqualsToken.WriteCode(output);
            NewValueNode.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
