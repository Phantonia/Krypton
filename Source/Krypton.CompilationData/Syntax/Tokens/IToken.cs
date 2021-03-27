using System.IO;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public interface IToken : IWritable
    {
        public abstract Trivia LeadingTrivia { get; }

        public abstract int LineNumber { get; }

        public abstract string Text { get; }
    }
}