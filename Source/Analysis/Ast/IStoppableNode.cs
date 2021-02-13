using Krypton.Analysis.Ast.TypeSpecs;

namespace Krypton.Analysis.Ast
{
    public interface IReturnableNode : INode
    {
        StatementCollectionNode BodyNode { get; }

        TypeSpecNode? ReturnTypeNode { get; }
    }
}
