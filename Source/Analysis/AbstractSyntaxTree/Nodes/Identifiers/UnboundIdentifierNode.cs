using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers
{
    public sealed class UnboundIdentifierNode : IdentifierNode
    {
        public UnboundIdentifierNode(string identifier, int lineNumber) : base(identifier, lineNumber) { }

        public override UnboundIdentifierNode Clone() => new(Identifier, LineNumber);

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
