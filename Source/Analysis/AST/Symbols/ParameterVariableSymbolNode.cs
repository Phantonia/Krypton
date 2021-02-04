namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class ParameterVariableSymbolNode : VariableSymbolNode
    {
        internal ParameterVariableSymbolNode(string name,
                                             TypeSymbolNode? type,
                                             int lineNumber,
                                             int index) : base(name, type, lineNumber, index) { }
    }
}
