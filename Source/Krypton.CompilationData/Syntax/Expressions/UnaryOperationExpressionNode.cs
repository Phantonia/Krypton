using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class UnaryOperationExpressionNode : ExpressionNode
    {
        public UnaryOperationExpressionNode(OperatorToken operatorToken,
                                            ExpressionNode operandNode)
            : this(operatorToken, operandNode, parent: null) { }

        public UnaryOperationExpressionNode(OperatorToken operatorToken,
                                            ExpressionNode operandNode,
                                            SyntaxNode? parent)
            : base(parent)
        {
            Debug.Assert(operatorToken.IsUnary);

            OperatorToken = operatorToken;
            OperandNode = operandNode.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; }

        public OperatorToken OperatorToken { get; }

        public UnaryOperationExpressionNode WithChildren(OperatorToken? operatorToken,
                                                         ExpressionNode? operandNode)
            => new(operatorToken ?? OperatorToken,
                   operandNode ?? OperandNode);

        public override UnaryOperationExpressionNode WithParent(SyntaxNode newParent)
            => new(OperatorToken, OperandNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            OperatorToken.WriteCode(output);
            OperandNode.WriteCode(output);
        }
    }
}
