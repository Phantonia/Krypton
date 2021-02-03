using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class LocalVariableSymbolNode : VariableSymbolNode
    {
        internal LocalVariableSymbolNode(string name, TypeSymbolNode? type, int lineNumber) : base(name, type, lineNumber) { }

        public void SpecifyType(TypeSymbolNode type)
        {
            Debug.Assert(TypeNode == null);
            TypeNode = type;
        }
    }
}
