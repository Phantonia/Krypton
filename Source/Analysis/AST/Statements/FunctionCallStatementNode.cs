using Krypton.Analysis.Ast.Expressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallStatementNode(FunctionCallExpressionNode expression, int lineNumber) : base(lineNumber)
        {
            this.UnderlyingFunctionCallExpressionNode = expression;
            this.UnderlyingFunctionCallExpressionNode.Parent = this;
        }

        public ReadOnlyCollection<ExpressionNode>? Arguments => UnderlyingFunctionCallExpressionNode.Arguments;

        public ExpressionNode FunctionExpression => UnderlyingFunctionCallExpressionNode.FunctionExpression;

        public FunctionCallExpressionNode UnderlyingFunctionCallExpressionNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            UnderlyingFunctionCallExpressionNode.PopulateBranches(list);
        }
    }
}
