using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    // Explicitly intended to be derived
    public class FunctionSymbolNode : SymbolNode
    {
        internal FunctionSymbolNode(string name,
                                    IEnumerable<ParameterSymbolNode> parameters,
                                    TypeSymbolNode? returnType,
                                    int lineNumber,
                                    int index) : base(name, lineNumber, index)
        {
            ParameterNodes = parameters.MakeReadOnly();
            ReturnTypeNode = returnType;
        }

        public ReadOnlyList<ParameterSymbolNode> ParameterNodes { get; }

        public TypeSymbolNode? ReturnTypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);

            foreach (ParameterSymbolNode parameter in ParameterNodes)
            {
                parameter.PopulateBranches(list);
            }
        }
    }
}
