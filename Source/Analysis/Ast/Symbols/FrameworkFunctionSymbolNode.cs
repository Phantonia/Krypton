using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class FrameworkFunctionSymbolNode : FunctionSymbolNode
    {
        internal FrameworkFunctionSymbolNode(string name,
                                             IEnumerable<ParameterSymbolNode> parameters,
                                             TypeSymbolNode? returnType,
                                             FunctionCallGenerator generator,
                                             int lineNumber,
                                             int index) : base(name, parameters, returnType, lineNumber, index)
        {
            Generator = generator;
        }

        public FunctionCallGenerator Generator { get; }
    }
}
