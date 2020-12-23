using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Types
{
    public sealed class IdentifierTypeNode : TypeNode
    {
        public IdentifierTypeNode(string identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new IdentifierNode(identifier, lineNumber)
            {
                Parent = this
            };
        }

        private IdentifierTypeNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public override IdentifierTypeNode Clone()
        {
            return new(IdentifierNode.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
        }
    }
}
