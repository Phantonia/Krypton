using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.Ast.Expressions
{
    public sealed class FunctionCallExpressionNode : ExpressionNode
    {
        public FunctionCallExpressionNode(ExpressionNode functionExpression, int lineNumber) : base(lineNumber)
        {
            FunctionExpression = functionExpression;
            FunctionExpression.Parent = this;
        }

        public FunctionCallExpressionNode(ExpressionNode functionExpression, IEnumerable<ExpressionNode>? arguments, int lineNumber) : base(lineNumber)
        {
            FunctionExpression = functionExpression;
            Arguments = (arguments as IList<ExpressionNode>) ?? arguments?.ToList();

            if (arguments != null)
            {
                foreach (ExpressionNode argument in arguments)
                {
                    argument.Parent = this;
                }
            }
        }

        // Null if the function is called without arguments
        public IList<ExpressionNode>? Arguments { get; } = null;

        public ExpressionNode FunctionExpression { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            FunctionExpression.PopulateBranches(list);

            if (Arguments != null)
            {
                foreach (ExpressionNode argument in Arguments)
                {
                    argument.PopulateBranches(list);
                }
            }
        }
    }
}
