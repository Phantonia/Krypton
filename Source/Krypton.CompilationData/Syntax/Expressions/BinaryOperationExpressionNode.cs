using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class BinaryOperationExpressionNode : ExpressionNode
    {
        public BinaryOperationExpressionNode(ExpressionNode leftOperand,
                                             OperatorToken @operator,
                                             ExpressionNode rightOperand,
                                             SyntaxNode? parent = null)
            : base(parent)
        {
            Debug.Assert(@operator.IsBinary);

            LeftOperandNode = leftOperand.WithParent(this);
            OperatorToken = @operator;
            RightOperandNode = rightOperand.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode LeftOperandNode { get; }

        public OperatorToken OperatorToken { get; }

        public ExpressionNode RightOperandNode { get; }

        public override TypedExpressionNode<BinaryOperationExpressionNode> Bind(TypeSymbol type)
            => new(this, type);

        public BinaryOperationExpressionNode WithChildren(ExpressionNode? leftOperand = null,
                                                          OperatorToken? @operator = null,
                                                          ExpressionNode? rightOperand = null)
            => new(leftOperand ?? LeftOperandNode,
                   @operator ?? OperatorToken,
                   rightOperand ?? RightOperandNode);

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
