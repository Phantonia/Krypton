using Krypton.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.Ast.Expressions
{
    public sealed class FunctionCallExpressionNode : ExpressionNode
    {
        internal FunctionCallExpressionNode(ExpressionNode functionExpression, int lineNumber) : base(lineNumber)
        {
            FunctionExpressionNode = functionExpression;
            FunctionExpressionNode.ParentNode = this;
        }

        internal FunctionCallExpressionNode(ExpressionNode functionExpression, IEnumerable<ExpressionNode>? arguments, int lineNumber) : base(lineNumber)
        {
            FunctionExpressionNode = functionExpression;
            ArgumentNodes = ((arguments as IList<ExpressionNode>) ?? arguments?.ToList())?.MakeReadOnly() ?? new ReadOnlyList<ExpressionNode>();

            if (arguments != null)
            {
                foreach (ExpressionNode argument in arguments)
                {
                    argument.ParentNode = this;
                }
            }
        }

        public ReadOnlyList<ExpressionNode> ArgumentNodes { get; } = default;

        public ExpressionNode FunctionExpressionNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            FunctionExpressionNode.PopulateBranches(list);

            foreach (ExpressionNode argument in ArgumentNodes)
            {
                argument.PopulateBranches(list);
            }
        }
    }
}
