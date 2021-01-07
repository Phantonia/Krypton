using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public sealed class ScriptNode : Node
    {
        public ScriptNode(BlockStatementNode statements, int lineNumber) : base(lineNumber)
        {
            Statements = statements;
        }

        public BlockStatementNode Statements { get; }

        public override ScriptNode Clone()
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
