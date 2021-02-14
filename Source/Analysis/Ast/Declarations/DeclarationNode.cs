using Krypton.Analysis.Ast.Identifiers;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Declarations
{
    public abstract class DeclarationNode : Node
    {
        private protected DeclarationNode(IdentifierNode identifierNode, int lineNumber, int index) : base(lineNumber, index)
        {
            IdentifierNode = identifierNode;
            identifierNode.ParentNode = this;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public sealed override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            PopulateBranchesInternal(list);
        }

        protected virtual void PopulateBranchesInternal(List<Node> list) { }
    }
}
