namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public sealed class ParameterVariableSymbolNode : VariableSymbolNode
    {
        public ParameterVariableSymbolNode(string name, TypeSymbolNode? type, int lineNumber) : base(name, type, lineNumber) { }
    }
}
