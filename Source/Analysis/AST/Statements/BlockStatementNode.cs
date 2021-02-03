using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    [DebuggerDisplay("{GetType().Name}; StatementCount = {StatementNodes.Count}")]
    public sealed class BlockStatementNode : StatementNode, IParentStatementNode
    {
        internal BlockStatementNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            StatementNodes = statements;
        }

        public StatementCollectionNode StatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            StatementNodes.PopulateBranches(list);
        }
    }
}
