using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ProgramNode : Node
    {
        internal ProgramNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            TopLevelStatementNodes = statements;
        }

        public StatementCollectionNode TopLevelStatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            TopLevelStatementNodes.PopulateBranches(list);
        }
    }
}
