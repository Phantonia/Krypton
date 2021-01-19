using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    /* A BinaryOperationExpressionNode is any expression
     * with a concrete operator lexeme and a sub expression
     * on the left hand side and one on the right hand
     * side.
     */
    public abstract class BinaryOperationExpressionNode : ExpressionNode
    {
        protected private BinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(lineNumber)
        {
            Left = left;
            Left.Parent = this;
            Right = right;
            Right.Parent = this;
        }

        public ExpressionNode Left { get; }

        public ExpressionNode Right { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Left.PopulateBranches(list);
            Right.PopulateBranches(list);
        }
    }
}
