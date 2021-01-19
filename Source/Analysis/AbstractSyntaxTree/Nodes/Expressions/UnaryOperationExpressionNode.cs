using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public abstract class UnaryOperationExpressionNode : ExpressionNode
    {
        protected private UnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(lineNumber)
        {
            Operand = operand;
        }

        public ExpressionNode Operand { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Operand.PopulateBranches(list);
        }
    }
}
