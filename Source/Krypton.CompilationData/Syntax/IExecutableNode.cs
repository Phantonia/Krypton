using Krypton.CompilationData.Syntax.Types;

namespace Krypton.CompilationData.Syntax
{
    public interface IExecutableNode : ISyntaxNode
    {
        public abstract BodyNode BodyNode { get; }

        public abstract TypeNode? ReturnTypeNode { get; }
    }
}
