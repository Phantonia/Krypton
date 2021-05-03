using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;

namespace Krypton.CompilationData.Syntax.Tokens
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

namespace Krypton.CompilationData.Syntax
{
    internal static class SyntaxCharacterHelper
    {
        public static string ToText(this SyntaxCharacter syntaxCharacter)
        {
            return syntaxCharacter switch
            {
                SyntaxCharacter.ParenthesisOpening => "(",
                SyntaxCharacter.ParenthesisClosing => ")",
                SyntaxCharacter.BraceOpening => "{",
                SyntaxCharacter.BraceClosing => "}",
                SyntaxCharacter.SquareBracketOpening => "[",
                SyntaxCharacter.SquareBracketClosing => "]",
                SyntaxCharacter.Semicolon => ";",
                SyntaxCharacter.Comma => ",",
                SyntaxCharacter.Colon => ":",
                SyntaxCharacter.Dot => ".",
                SyntaxCharacter.Equals => "=",
                SyntaxCharacter.Underscore => "_",
                _ => OnFailure(),
            };

            static string OnFailure()
            {
                Debug.Fail(message: null);
                return null;
            }
        }
    }
}
