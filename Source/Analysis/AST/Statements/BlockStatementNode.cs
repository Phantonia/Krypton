using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class BlockStatementNode : StatementNode, IParentStatementNode
    {
        public BlockStatementNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            Statements = statements;
        }

        public StatementCollectionNode Statements { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Statements.PopulateBranches(list);
        }
    }
}
