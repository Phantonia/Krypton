using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Collections.Generic;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class OutStatementNode : StatementNode
    {
        public OutStatementNode(int lineNumber, ExpressionNode outExpression) : base(lineNumber)
        {
            OutExpression = outExpression;
        }

        public ExpressionNode OutExpression { get; }

        public override OutStatementNode Clone()
        {
            return new OutStatementNode(LineNumber, OutExpression);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append("console.log(");
            OutExpression.GenerateCode(stringBuilder);
            stringBuilder.Append(");\r\n");
        }

        protected override IEnumerable<Node> GetBranches()
        {
            yield return OutExpression;
        }
    }
}
