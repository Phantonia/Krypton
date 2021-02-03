using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class FunctionSymbolNode : SymbolNode
    {
        private protected FunctionSymbolNode(string name, IEnumerable<ParameterNode> parameters, TypeSymbolNode? returnType, int lineNumber) : base(name, lineNumber)
        {
            ParameterNodes = parameters.MakeReadOnly();
            ReturnTypeNode = returnType;
        }

        public ReadOnlyList<ParameterNode> ParameterNodes { get; }

        public TypeSymbolNode? ReturnTypeNode { get; }
    }
}
