using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public sealed class ScriptNode : Node
    {
        public ScriptNode() : base(1)
        {

        }

        public List<StatementNode> Statements { get; } = new();

        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            foreach (var statement in Statements)
            {
                statement.GenerateCode(stringBuilder);
            }
        }

        protected override IEnumerable<Node> GetBranches()
        {
            return base.GetBranches();
        }
    }
}
