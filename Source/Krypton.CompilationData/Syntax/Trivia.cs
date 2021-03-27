using System;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public readonly struct Trivia : IWritable
    {
        public Trivia(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public int EndIndex { get; }

        public int StartIndex { get; }

        public void WriteCode(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        public static Trivia Null => default;
    }
}
