using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallStatementNode(FunctionCallExpressionNode expression, int lineNumber) : base(lineNumber)
        {
            this.expression = expression;
            this.expression.Parent = this;
        }

        private readonly FunctionCallExpressionNode expression;

        public IList<ExpressionNode>? Arguments => expression.Arguments;

        public ExpressionNode FunctionExpression => expression.FunctionExpression;

        public override FunctionCallStatementNode Clone()
        {
            return new(expression.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            expression.PopulateBranches(list);
        }
    }
}
