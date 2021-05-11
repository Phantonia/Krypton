using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class ReservedKeywordToken : Token
    {
        public ReservedKeywordToken(ReservedKeyword keyword,
                                    ReadOnlyMemory<char> text,
                                    int lineNumber,
                                    Trivia leadingTrivia)
            : base(text, lineNumber, leadingTrivia)
        {
            Keyword = keyword;
        }

        public ReservedKeyword Keyword { get; }
    }
}
