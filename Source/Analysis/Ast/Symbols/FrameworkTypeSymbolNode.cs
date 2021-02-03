using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class FrameworkTypeSymbolNode : TypeSymbolNode
    {
        internal FrameworkTypeSymbolNode(FrameworkType frameworkType, string name, int lineNumber) : base(name, lineNumber)
        {
            FrameworkType = frameworkType;
        }

        public FrameworkType FrameworkType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
