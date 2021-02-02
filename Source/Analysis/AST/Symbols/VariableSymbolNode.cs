using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class VariableSymbolNode : SymbolNode
    {
        private protected VariableSymbolNode(string name, TypeSymbolNode? type, int lineNumber) : base(name, lineNumber)
        {
            Type = type;
        }

        public virtual TypeSymbolNode? Type { get; protected set; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Type?.PopulateBranches(list);
        }
    }
}
