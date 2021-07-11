using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record BinaryOperationExpressionNode : ExpressionNode
    {
        public BinaryOperationExpressionNode(ExpressionNode leftOperand,
                                             OperatorToken @operator,
                                             ExpressionNode rightOperand)
        {
            Debug.Assert(@operator.IsBinary);

            LeftOperandNode = leftOperand;
            OperatorToken = @operator;
            RightOperandNode = rightOperand;
        }

        public override bool IsLeaf => false;

        public ExpressionNode LeftOperandNode { get; init; }

        public override Token LexicallyFirstToken => LeftOperandNode.LexicallyFirstToken;

        public Operator Operator => OperatorToken.Operator;

        public OperatorToken OperatorToken { get; init; }

        public ExpressionNode RightOperandNode { get; init; }

        public BoundExpressionNode<BinaryOperationExpressionNode, BinaryOperationSymbol> Bind(BinaryOperationSymbol operationSymbol)
            => new(this, operationSymbol);

        public override void WriteCode(TextWriter output)
        {
            LeftOperandNode.WriteCode(output);
            OperatorToken.WriteCode(output);
            RightOperandNode.WriteCode(output);
        }
    }
}
