using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Declarations
{
    public sealed class ParameterDeclarationNode : DeclarationNode
    {
        internal ParameterDeclarationNode(IdentifierNode identifierNode, TypeSpecNode type, int lineNumber, int index) : base(identifierNode, lineNumber, index)
        {
            TypeNode = type;
        }

        public TypeSpecNode TypeNode { get; }

        protected override void PopulateBranchesInternal(List<Node> list)
        {
            TypeNode.PopulateBranches(list);
        }
    }
}
