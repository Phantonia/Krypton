using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Diagnostics;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{

    public sealed class LocalVariableSymbolNode : VariableSymbolNode
    {
        public LocalVariableSymbolNode(string name, TypeSymbolNode? type, int lineNumber) : base(name, type, lineNumber) { }

        public void SpecifyType(TypeSymbolNode type)
        {
            Debug.Assert(Type == null);
            Type = type;
        }
    }
}
