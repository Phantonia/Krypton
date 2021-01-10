using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public sealed class ParameterVariableSymbolNode : VariableSymbolNode
    {
        public ParameterVariableSymbolNode(string name, TypeNode? type, int lineNumber) : base(name, type, lineNumber) { }

        public override ParameterVariableSymbolNode Clone() => new(Name, Type, LineNumber);
    }
}
