using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class BuiltinTypeSymbolNode : TypeSymbolNode
    {
        public BuiltinTypeSymbolNode(FrameworkType builtinType, string name, int lineNumber) : base(name, lineNumber)
        {
            BuiltinType = builtinType;
        }

        public FrameworkType BuiltinType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
