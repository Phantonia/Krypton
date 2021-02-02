using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    /* A BinaryOperationExpressionNode is any expression
     * with a concrete operator lexeme and a sub expression
     * on the left hand side and one on the right hand
     * side.
     */
    public class BinaryOperationExpressionNode : ExpressionNode
    {
        internal BinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, Operator @operator, int lineNumber) : base(lineNumber)
        {
            Left = left;
            Left.Parent = this;
            Right = right;
            Operator = @operator;
            Right.Parent = this;
        }

        public ExpressionNode Left { get; }

        public Operator Operator { get; }

        public ExpressionNode Right { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Left.PopulateBranches(list);
            Right.PopulateBranches(list);
        }
    }
}
