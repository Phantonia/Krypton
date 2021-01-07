using Krypton.Analysis.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class BlockStatementNode : StatementNode
    {
        public BlockStatementNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            Statements = statements;
        }

        public StatementCollectionNode Statements { get; }

        public override BlockStatementNode Clone()
        {
            return new(Statements.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Statements.PopulateBranches(list);
        }
    }
}
