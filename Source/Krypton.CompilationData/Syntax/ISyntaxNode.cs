namespace Krypton.CompilationData.Syntax
{
    public interface ISyntaxNode : IWritable
    {
        public abstract bool IsLeaf { get; }
    }
}