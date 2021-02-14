using Krypton.Analysis.Ast.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class ReturnStatementNode : StatementNode
    {
        public ReturnStatementNode(int lineNumber, int index) : base(lineNumber, index) { }

        public ReturnStatementNode(ExpressionNode returnExpression,
                                   int lineNumber,
                                   int index) : base(lineNumber, index)
        {
            ReturnExpressionNode = returnExpression;

            if (ReturnExpressionNode != null)
            {
                ReturnExpressionNode.ParentNode = this;
            }
        }

        public ExpressionNode? ReturnExpressionNode { get; } = null;

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            ReturnExpressionNode?.PopulateBranches(list);
        }
    }
}
