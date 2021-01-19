using Krypton.Analysis.AST.Identifiers;
using Krypton.Analysis.AST.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.AST
{
    /* A ParameterNode represents the declaration of
     * a parameter of a function. It is used by 
     */
    public sealed class ParameterNode : Node
    {
        public ParameterNode(string identifier, TypeSymbolNode type, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber) { Parent = this };
            Type = type;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public TypeSymbolNode Type { get; }

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
