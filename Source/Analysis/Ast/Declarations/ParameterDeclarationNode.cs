using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Declarations
{
    public sealed class ParameterDeclarationNode : DeclarationNode
    {
        internal ParameterDeclarationNode(IdentifierNode identifierNode, TypeSpecNode type, int lineNumber, int index) : base(identifierNode, lineNumber, index)
        {
            Type = type;
        }

        public TypeSpecNode Type { get; }

        protected override void PopulateBranchesInternal(List<Node> list)
        {
            Type.PopulateBranches(list);
        }
    }
}
