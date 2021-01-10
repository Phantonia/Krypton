using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Diagnostics;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{

    public sealed class LocalVariableSymbolNode : VariableSymbolNode
    {
        public LocalVariableSymbolNode(string name, TypeNode? type, int lineNumber) : base(name, type, lineNumber) { }

        public override LocalVariableSymbolNode Clone() => new(Name, Type, LineNumber);

        public void SpecifyType(TypeNode type)
        {
            Debug.Assert(Type == null);
            Type = type;
        }
    }
}
