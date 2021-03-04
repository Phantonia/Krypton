using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class PropertySymbolNode : SymbolNode
    {
        public PropertySymbolNode(string identifier,
                                  TypeSymbolNode type,
                                  CodeGenerationInformation codeGenerationInfo,
                                  int lineNumber,
                                  int index) : base(identifier, lineNumber, index)
        {
            TypeNode = type;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public TypeSymbolNode TypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
