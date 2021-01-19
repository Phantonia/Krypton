using Krypton.Analysis.AST.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.AST.Statements
{
    public sealed class WhileStatementNode : StatementNode, IParentStatementNode
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

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Condition.PopulateBranches(list);
            Statements.PopulateBranches(list);
        }
    }
}
