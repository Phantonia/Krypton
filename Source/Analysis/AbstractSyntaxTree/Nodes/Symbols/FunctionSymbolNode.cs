using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public abstract class FunctionSymbolNode : SymbolNode
    {
        protected FunctionSymbolNode(string name, IEnumerable<ParameterNode> parameters, TypeNode? returnType, int lineNumber) : base(name, lineNumber)
        {
            Parameters = parameters.ToImmutableList();
            ReturnType = returnType;
        }

        public ImmutableList<ParameterNode> Parameters { get; }

        public TypeNode? ReturnType { get; }
    }
}
