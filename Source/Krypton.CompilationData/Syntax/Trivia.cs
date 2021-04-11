using System;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public readonly struct Trivia : IWritable
    {
        public Trivia(ReadOnlyMemory<char> text)
        {
            Text = text;
        }

        public ReadOnlyMemory<char> Text { get; }

        public override string ToString()
            => new(Text.Span);

        public void WriteCode(TextWriter textWriter)
        {
            textWriter.Write(Text.Span);
        }

        public static Trivia Null => default;
    }
}
