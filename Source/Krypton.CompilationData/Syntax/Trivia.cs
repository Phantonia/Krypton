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

        public void WriteCode(TextWriter textWriter)
        {
            foreach (char c in Text.Span)
            {
                textWriter.Write(c);
            }
        }

        public static Trivia Null => default;
    }
}
