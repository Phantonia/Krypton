using Krypton.Analysis.AST.TypeSpecs;
using System.Diagnostics;

namespace Krypton.Analysis.AST.Symbols
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
