namespace Krypton.Analysis.Lexical.Lexemes
{
    public enum SyntaxCharacter
    {
        None = 0,

        // Brackets
        ParenthesisOpening, // (
        ParenthesisClosing, // )
        BraceOpening, // {
        BraceClosing, // }
        SquareBracketOpening, // [
        SquareBracketClosing, // ]

        // Punctuation
        Semicolon, // ;
        Comma, // ,
        Colon, // :
        Dot, // .

        // Other
        Equals, // =
        Underscore, // _
    }
}
