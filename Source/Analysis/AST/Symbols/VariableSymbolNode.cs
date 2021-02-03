using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class VariableSymbolNode : SymbolNode
    {
        private protected VariableSymbolNode(string identifier, TypeSymbolNode? typeNode, int lineNumber) : base(identifier, lineNumber)
        {
            TypeNode = typeNode;
        }

        public virtual TypeSymbolNode? TypeNode { get; protected set; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
