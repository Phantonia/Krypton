using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public interface ISyntaxNode : IWritable
    {
        public abstract bool IsLeaf { get; }

        public abstract SyntaxNode? ParentNode { get; }

        public abstract SyntaxNode WithParent(SyntaxNode newParent);
    }
}