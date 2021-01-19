using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.TypeSpecs
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
