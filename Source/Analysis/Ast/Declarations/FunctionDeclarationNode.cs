using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Declarations
{
    public sealed class FunctionDeclarationNode : DeclarationNode
    {
        internal FunctionDeclarationNode(IdentifierNode identifierNode,
                                         IList<ParameterDeclarationNode>? parameters,
                                         TypeSpecNode? returnType,
                                         StatementCollectionNode body,
                                         int lineNumber,
                                         int index) : base(identifierNode, lineNumber, index)
        {
            ParameterNodes = parameters.MakeReadOnly();
            ReturnTypeNode = returnType;
            BodyNode = body;
        }

        public StatementCollectionNode BodyNode { get; }

        public ReadOnlyList<ParameterDeclarationNode> ParameterNodes { get; }

        public TypeSpecNode? ReturnTypeNode { get; }

        protected override void PopulateBranchesInternal(List<Node> list)
        {
            foreach (ParameterDeclarationNode parameter in ParameterNodes)
            {
                parameter.PopulateBranches(list);
            }

            BodyNode.PopulateBranches(list);
            ReturnTypeNode?.PopulateBranches(list);
        }
    }
}
