using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.TypeSpecs;

namespace Krypton.Analysis.Ast.Declarations
{
    public sealed class ConstantDeclarationNode : DeclarationNode
    {
        internal ConstantDeclarationNode(IdentifierNode identifierNode,
                                         TypeSpecNode? type,
                                         ExpressionNode value,
                                         int lineNumber,
                                         int index) : base(identifierNode, lineNumber, index)
        {
            TypeSpecNode = type;
            ValueNode = value;
        }

        public TypeSpecNode? TypeSpecNode { get; }

        public ExpressionNode ValueNode { get; }
    }
}
