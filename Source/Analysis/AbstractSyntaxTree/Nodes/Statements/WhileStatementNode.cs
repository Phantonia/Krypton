using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class WhileStatementNode : StatementNode
    {
        public WhileStatementNode(ExpressionNode condition, BlockStatementNode statements, int lineNumber) : base(lineNumber)
        {
            Condition = condition;
            Statements = statements;
        }

        public ExpressionNode Condition { get; }

        public BlockStatementNode Statements { get; }

        public override WhileStatementNode Clone()
        {
            return new(Condition.Clone(), Statements.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append("while (");
            Condition.GenerateCode(stringBuilder);
            stringBuilder.Append(")\r\n");
            Statements.GenerateCode(stringBuilder);
        }
    }
}
