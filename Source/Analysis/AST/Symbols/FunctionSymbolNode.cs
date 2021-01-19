using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class FunctionSymbolNode : SymbolNode
    {
        protected FunctionSymbolNode(string name, IEnumerable<ParameterNode> parameters, TypeSpecNode? returnType, int lineNumber) : base(name, lineNumber)
        {
            Parameters = parameters.ToImmutableList();
            ReturnType = returnType;
        }

        public ImmutableList<ParameterNode> Parameters { get; }

        public TypeSpecNode? ReturnType { get; }
    }
}
