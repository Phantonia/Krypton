using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public class UnaryOperationExpressionNode : ExpressionNode
    {
        internal UnaryOperationExpressionNode(ExpressionNode operand, Operator @operator, int lineNumber) : base(lineNumber)
        {
            Operand = operand;
            Operator = @operator;
        }

        public ExpressionNode Operand { get; }

        public Operator Operator { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Operand.PopulateBranches(list);
        }
    }
}
