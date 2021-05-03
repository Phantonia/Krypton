using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record UnaryOperationExpressionNode : ExpressionNode
    {
        public UnaryOperationExpressionNode(OperatorToken @operator,
                                            ExpressionNode operand)
        {
            Debug.Assert(@operator.IsUnary);

            OperatorToken = @operator;
            OperandNode = operand;
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; init; }

        public Operator Operator => OperatorToken.Operator;

        public OperatorToken OperatorToken { get; init; }

        public BoundExpressionNode<UnaryOperationExpressionNode, UnaryOperationSymbol> Bind(UnaryOperationSymbol operation)
            => new(this, operation);

        public override void WriteCode(TextWriter output)
        {
            OperatorToken.WriteCode(output);
            OperandNode.WriteCode(output);
        }
    }
}
