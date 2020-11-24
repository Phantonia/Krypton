using System;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public sealed class ScriptNode : Node
    {
        public ScriptNode() : base(1)
        {

        }

        public override Node Clone()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Node> GetBranches()
        {
            return base.GetBranches();
        }
    }
}
