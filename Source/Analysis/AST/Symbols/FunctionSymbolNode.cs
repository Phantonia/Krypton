using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class FunctionSymbolNode : SymbolNode
    {
        private protected FunctionSymbolNode(string name,
                                             IEnumerable<ParameterNode> parameters,
                                             TypeSymbolNode? returnType,
                                             int lineNumber,
                                             int index) : base(name, lineNumber, index)
        {
            ParameterNodes = parameters.MakeReadOnly();
            ReturnTypeNode = returnType;
        }

        public ReadOnlyList<ParameterNode> ParameterNodes { get; }

        public TypeSymbolNode? ReturnTypeNode { get; }
    }
}
