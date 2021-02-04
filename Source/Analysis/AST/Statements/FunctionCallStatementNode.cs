using Krypton.Analysis.Ast.Expressions;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class FunctionCallStatementNode : StatementNode
    {
        internal FunctionCallStatementNode(FunctionCallExpressionNode expression, int lineNumber, int index) : base(lineNumber, index)
        {
            UnderlyingFunctionCallExpressionNode = expression;
            UnderlyingFunctionCallExpressionNode.ParentNode = this;
        }

        public ReadOnlyList<ExpressionNode>? ArgumentNodes => UnderlyingFunctionCallExpressionNode.ArgumentNodes;

        public ExpressionNode FunctionExpressionNode => UnderlyingFunctionCallExpressionNode.FunctionExpressionNode;

        public FunctionCallExpressionNode UnderlyingFunctionCallExpressionNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            UnderlyingFunctionCallExpressionNode.PopulateBranches(list);
        }
    }
}
