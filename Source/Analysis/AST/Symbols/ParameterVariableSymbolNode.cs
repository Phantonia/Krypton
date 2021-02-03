namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class ParameterVariableSymbolNode : VariableSymbolNode
    {
        internal ParameterVariableSymbolNode(string name, TypeSymbolNode? type, int lineNumber) : base(name, type, lineNumber) { }
    }
}
