using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public interface ISyntaxNode
    {
        public abstract bool IsLeaf { get; }

        public abstract SyntaxNode? Parent { get; }

        public abstract SyntaxNode WithParent(SyntaxNode newParent);

        public abstract void WriteCode(TextWriter output);
    }
}