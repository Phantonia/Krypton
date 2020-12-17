namespace Krypton.Analysis.Errors
{
    public enum ErrorCode : ushort
    {
        None = 0,

        // Lexical errors
        UnknownLexeme = 1,
        UnclosedStringLiteral = 2,
        UnclosedCharLiteral = 3,
        UnknownEscapeSequence = 4,
        HexLiteralWithMixedCase = 5,

        // Grammatical errors
        ExpectedSemicolon = 101,
        ExpectedClosingParenthesis = 102,
        UnexpectedExpressionTerm = 103,
        ExpectedCommaOrClosingParenthesis = 104,
        ExpectedIdentifier = 105,
        ExpectedEqualsOrSemicolon = 106,
    }
}
