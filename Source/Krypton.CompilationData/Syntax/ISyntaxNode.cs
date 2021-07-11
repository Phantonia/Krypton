using Krypton.CompilationData.Syntax.Tokens;

namespace Krypton.CompilationData.Syntax
{
    public interface ISyntaxNode : IWritable
    {
        public abstract bool IsLeaf { get; }

        Token LexicallyFirstToken { get; }
    }
}