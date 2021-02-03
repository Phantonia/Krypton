using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public class UnaryOperationExpressionNode : ExpressionNode
    {
        internal UnaryOperationExpressionNode(ExpressionNode operand, Operator @operator, int lineNumber) : base(lineNumber)
        {
            OperandNode = operand;
            Operator = @operator;
        }

        public ExpressionNode OperandNode { get; }

        public Operator Operator { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            OperandNode.PopulateBranches(list);
        }
    }
}
