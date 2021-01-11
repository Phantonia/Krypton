using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public sealed class ParameterNode : Node
    {
        public ParameterNode(string identifier, TypeSymbolNode type, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber) { Parent = this };
            Type = type;
        }

        private ParameterNode(IdentifierNode identifierNode, TypeSymbolNode type, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifierNode;
            Type = type;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public TypeSymbolNode Type { get; }

        public override ParameterNode Clone() => new(IdentifierNode.Clone(), Type.Clone(), LineNumber);

        public ParameterVariableSymbolNode CreateParameter()
        {
            ParameterVariableSymbolNode symbol = new(Identifier, Type, LineNumber);
            IdentifierNode = new BoundIdentifierNode(Identifier, symbol, IdentifierNode.LineNumber) { Parent = this };
            return symbol;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            Type.PopulateBranches(list);
        }
    }
}
