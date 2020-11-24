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
    }
}
