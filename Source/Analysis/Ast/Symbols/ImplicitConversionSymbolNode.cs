using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class ImplicitConversionSymbolNode : SymbolNode
    {
        public ImplicitConversionSymbolNode(TypeSymbolNode targetType,
                                            int lineNumber,
                                            int index) : base(string.Empty, lineNumber, index)
        {
            TargetTypeNode = targetType;
        }

        public TypeSymbolNode TargetTypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
