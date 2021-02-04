using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class LocalVariableSymbolNode : VariableSymbolNode
    {
        internal LocalVariableSymbolNode(string name,
                                         TypeSymbolNode? type,
                                         int lineNumber,
                                         int index) : base(name, type, lineNumber, index) { }

        public void SpecifyType(TypeSymbolNode type)
        {
            Debug.Assert(TypeNode == null);
            TypeNode = type;
        }
    }
}
