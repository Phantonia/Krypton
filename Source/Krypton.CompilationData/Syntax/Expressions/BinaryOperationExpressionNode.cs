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

        public OperatorToken OperatorToken { get; init; }

        public ExpressionNode RightOperandNode { get; init; }

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

        public override void WriteCode(TextWriter output)
        {
            LeftOperandNode.WriteCode(output);
            OperatorToken.WriteCode(output);
            RightOperandNode.WriteCode(output);
        }
    }
}
