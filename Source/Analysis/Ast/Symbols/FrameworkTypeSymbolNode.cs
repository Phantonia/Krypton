using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class FrameworkTypeSymbolNode : TypeSymbolNode
    {
        internal FrameworkTypeSymbolNode(FrameworkType frameworkType,
                                         string name,
                                         int lineNumber,
                                         int index) : base(name, lineNumber, index)
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
