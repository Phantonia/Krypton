using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public abstract class VariableSymbolNode : SymbolNode
    {
        protected VariableSymbolNode(string name, TypeNode? type, int lineNumber) : base(name, lineNumber)
        {
            Type = type;
        }

        public virtual TypeNode? Type { get; protected set; }

        public abstract override VariableSymbolNode Clone();

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            Type?.PopulateBranches(list);
        }
    }
}
