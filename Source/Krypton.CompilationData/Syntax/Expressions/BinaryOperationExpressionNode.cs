using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class BinaryOperationExpressionNode : ExpressionNode
    {
        public BinaryOperationExpressionNode(ExpressionNode leftOperandNode,
                                             OperatorToken operatorToken,
                                             ExpressionNode rightOperandNode)
            : this(leftOperandNode, operatorToken, rightOperandNode, parent: null) { }

        public BinaryOperationExpressionNode(ExpressionNode leftOperandNode,
                                             OperatorToken operatorToken,
                                             ExpressionNode rightOperandNode,
                                             SyntaxNode? parent)
            : base(parent)
        {
            Debug.Assert(operatorToken.IsBinary);

            LeftOperandNode = leftOperandNode.WithParent(this);
            OperatorToken = operatorToken;
            RightOperandNode = rightOperandNode.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode LeftOperandNode { get; }

        public OperatorToken OperatorToken { get; }

        public ExpressionNode RightOperandNode { get; }

        public BinaryOperationExpressionNode WithChildren(ExpressionNode? leftOperandNode = null,
                                                          OperatorToken? operatorToken = null,
                                                          ExpressionNode? rightOperandNode = null)
            => new(leftOperandNode ?? LeftOperandNode,
                   operatorToken ?? OperatorToken,
                   rightOperandNode ?? RightOperandNode);

        public override BinaryOperationExpressionNode WithParent(SyntaxNode newParent)
            => new(LeftOperandNode, OperatorToken, RightOperandNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            LeftOperandNode.WriteCode(output);
            OperatorToken.WriteCode(output);
            RightOperandNode.WriteCode(output);
        }
    }
}
