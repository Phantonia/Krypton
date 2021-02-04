using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Identifiers
{
    public sealed class UnboundIdentifierNode : IdentifierNode
    {
        internal UnboundIdentifierNode(string identifier, int lineNumber, int index) : base(identifier, lineNumber, index) { }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
