using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public sealed class BuiltinFunctionSymbolNode : FunctionSymbolNode
    {
        public BuiltinFunctionSymbolNode(BuiltinFunction builtinFunction, string name, IEnumerable<ParameterNode> parameters, TypeNode? returnType, int lineNumber) : base(name, parameters, returnType, lineNumber)
        {
            BuiltinFunction = builtinFunction;
        }

        public BuiltinFunction BuiltinFunction { get; }

        public override BuiltinFunctionSymbolNode Clone() => new(BuiltinFunction, Name, Parameters.Select(p => p.Clone()), ReturnType, LineNumber);

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
