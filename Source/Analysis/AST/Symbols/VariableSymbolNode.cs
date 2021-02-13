using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class VariableSymbolNode : SymbolNode
    {
        internal VariableSymbolNode(string identifier,
                                    TypeSymbolNode? typeNode,
                                    bool isReadOnly,
                                    int lineNumber,
                                    int index) : base(identifier, lineNumber, index)
        {
            TypeNode = typeNode;
            IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; }

        public TypeSymbolNode? TypeNode { get; private set; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
