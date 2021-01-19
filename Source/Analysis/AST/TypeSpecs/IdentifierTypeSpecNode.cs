using Krypton.Analysis.AST.Identifiers;
using Krypton.Analysis.AST.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.AST.TypeSpecs
{
    public sealed class IdentifierTypeSpecNode : TypeSpecNode, IBindable
    {
        public IdentifierTypeSpecNode(string identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber)
            {
                Parent = this
            };
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public void Bind(TypeSymbolNode symbol)
        {
            IdentifierNode = new BoundIdentifierNode(Identifier, symbol, IdentifierNode.LineNumber) { Parent = this };
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
        }

        void IBindable.Bind(SymbolNode symbol)
        {
            TypeSymbolNode? type = symbol as TypeSymbolNode;
            Debug.Assert(type != null);
            Bind(type);
        }
    }
}
