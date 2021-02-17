using Krypton.Analysis.Ast.TypeSpecs;

namespace Krypton.Analysis.Ast
{
    public interface IExecutableNode : INode
    {
        StatementCollectionNode BodyNode { get; }

        TypeSpecNode? ReturnTypeNode { get; }
    }
}
