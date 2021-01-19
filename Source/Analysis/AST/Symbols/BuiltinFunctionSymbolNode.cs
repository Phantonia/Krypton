using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class BuiltinFunctionSymbolNode : FunctionSymbolNode
    {
        public BuiltinFunctionSymbolNode(BuiltinFunction builtinFunction, string name, IEnumerable<ParameterNode> parameters, TypeSpecNode? returnType, int lineNumber) : base(name, parameters, returnType, lineNumber)
        {
            BuiltinFunction = builtinFunction;
        }

        public BuiltinFunction BuiltinFunction { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
