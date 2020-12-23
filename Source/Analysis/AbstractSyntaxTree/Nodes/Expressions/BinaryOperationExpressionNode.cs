using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public abstract class BinaryOperationExpressionNode : ExpressionNode
    {
        protected BinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(lineNumber)
        {
            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
        }

        public ExpressionNode Left { get; }

        public ExpressionNode Right { get; }

        public abstract override BinaryOperationExpressionNode Clone();

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Left.PopulateBranches(list);
            Right.PopulateBranches(list);
        }
    }
}
