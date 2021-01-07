using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class WhileStatementNode : StatementNode
    {
        public WhileStatementNode(ExpressionNode condition, StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            Condition = condition;
            Condition.Parent = this;
            Statements = statements;
            Statements.Parent = this;
        }

        public ExpressionNode Condition { get; }

        public StatementCollectionNode Statements { get; }

        public override WhileStatementNode Clone()
        {
            return new(Condition.Clone(), Statements.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Condition.PopulateBranches(list);
            Statements.PopulateBranches(list);
        }
    }
}
