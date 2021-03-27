using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class UnaryOperationExpressionNode : ExpressionNode
    {
        public UnaryOperationExpressionNode(OperatorToken @operator,
                                            ExpressionNode operand,
                                            SyntaxNode? parent = null)
            : base(parent)
        {
            Debug.Assert(@operator.IsUnary);

            OperatorToken = @operator;
            OperandNode = operand.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; }

        public OperatorToken OperatorToken { get; }

        public override TypedExpressionNode<UnaryOperationExpressionNode> Type(TypeSymbol type)
            => new(this, type);

        public UnaryOperationExpressionNode WithChildren(OperatorToken? @operator = null,
                                                         ExpressionNode? operand = null)
            => new(@operator ?? OperatorToken,
                   operand ?? OperandNode);

        public override UnaryOperationExpressionNode WithParent(SyntaxNode newParent)
            => new(OperatorToken, OperandNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            OperatorToken.WriteCode(output);
            OperandNode.WriteCode(output);
        }
    }
}
