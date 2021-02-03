using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ParameterNode : Node
    {
        internal ParameterNode(string identifier, TypeSymbolNode type, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber) { ParentNode = this };
            TypeNode = type;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public TypeSymbolNode TypeNode { get; }

        public ParameterVariableSymbolNode CreateParameterSymbol()
        {
            ParameterVariableSymbolNode symbol = new(Identifier, TypeNode, LineNumber);
            IdentifierNode = new BoundIdentifierNode(Identifier, symbol, IdentifierNode.LineNumber) { ParentNode = this };
            return symbol;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            TypeNode.PopulateBranches(list);
        }
    }
}
