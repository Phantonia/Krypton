using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public interface IToken : IWritable
    {
        public abstract Trivia LeadingTrivia { get; }

        public abstract int LineNumber { get; }

        public abstract ReadOnlyMemory<char> Text { get; }
    }
}