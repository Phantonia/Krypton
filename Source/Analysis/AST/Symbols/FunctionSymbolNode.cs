using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class FunctionSymbolNode : SymbolNode
    {
        private protected FunctionSymbolNode(string name, IEnumerable<ParameterNode> parameters, TypeSymbolNode? returnType, int lineNumber) : base(name, lineNumber)
        {
            Parameters = parameters.ToImmutableList();
            ReturnType = returnType;
        }

        public ImmutableList<ParameterNode> Parameters { get; }

        public TypeSymbolNode? ReturnType { get; }
    }
}
