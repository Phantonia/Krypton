using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class ImplicitConversionSymbolNode : SymbolNode
    {
        public ImplicitConversionSymbolNode(TypeSymbolNode targetType,
                                            CodeGenerationInformation codeGenerationInfo,
                                            int lineNumber,
                                            int index) : base(string.Empty, lineNumber, index)
        {
            TargetTypeNode = targetType;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public TypeSymbolNode TargetTypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
