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

        public string TextToString()
            => new(Text.Span);

        public override string ToString()
            => TextToString();

        public void WriteCode(TextWriter textWriter)
        {
            textWriter.Write(Text.Span);
        }

        public static Trivia Null => default;
    }
}
