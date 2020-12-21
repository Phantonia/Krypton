using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Collections.Generic;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class FunctionCallStatementNode : StatementNode
    {
        public FunctionCallStatementNode(FunctionCallExpressionNode expression, int lineNumber) : base(lineNumber)
        {
            this.expression = expression;
        }

        private readonly FunctionCallExpressionNode expression;

        public List<ExpressionNode>? Arguments => expression.Arguments;

        public ExpressionNode FunctionExpression => expression.FunctionExpression;

        public override FunctionCallStatementNode Clone()
        {
            return new(expression.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            expression.GenerateCode(stringBuilder);
            stringBuilder.Append(";\r\n");
        }
    }
}
