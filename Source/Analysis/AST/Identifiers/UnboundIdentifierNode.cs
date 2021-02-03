using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Identifiers
{
    public sealed class UnboundIdentifierNode : IdentifierNode
    {
        internal UnboundIdentifierNode(string identifier, int lineNumber) : base(identifier, lineNumber) { }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
