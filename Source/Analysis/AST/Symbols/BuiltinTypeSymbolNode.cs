using Krypton.Analysis.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.AST.Symbols
{
    public sealed class BuiltinTypeSymbolNode : TypeSymbolNode
    {
        public BuiltinTypeSymbolNode(BuiltinType builtinType, string name, int lineNumber) : base(name, lineNumber)
        {
            BuiltinType = builtinType;
        }

        public BuiltinType BuiltinType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
