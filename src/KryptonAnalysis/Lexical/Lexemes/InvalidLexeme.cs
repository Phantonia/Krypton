using Krypton.Analysis.Errors;

namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class InvalidLexeme : Lexeme
    {
        public InvalidLexeme(string content, ErrorCode errorCode, int lineNumber) : base(lineNumber)
        {
            Content = content;
            ErrorCode = errorCode;
        }

        public override string Content { get; }

        public ErrorCode ErrorCode { get; }
    }
}
