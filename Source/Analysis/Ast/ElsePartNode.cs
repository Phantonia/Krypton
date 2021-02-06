using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ElsePartNode : Node
    {
        internal ElsePartNode(StatementCollectionNode statements, int lineNumber, int index) : base(lineNumber, index)
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
