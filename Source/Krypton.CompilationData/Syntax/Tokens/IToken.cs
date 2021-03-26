using System.IO;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public interface IToken
    {
        public abstract Trivia LeadingTrivia { get; }

        public abstract int LineNumber { get; }

        public abstract string Text { get; }

        public abstract void WriteCode(TextWriter textWriter);
    }
}