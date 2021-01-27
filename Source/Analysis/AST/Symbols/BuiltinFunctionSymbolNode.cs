using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class BuiltinFunctionSymbolNode : FunctionSymbolNode
    {
        public BuiltinFunctionSymbolNode(string name, IEnumerable<ParameterNode> parameters, TypeSymbolNode? returnType, FunctionCallGenerator generator, int lineNumber) : base(name, parameters, returnType, lineNumber)
        {
            Generator = generator;
        }

        public FunctionCallGenerator Generator { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
