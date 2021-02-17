using System;

namespace Krypton.Analysis.Lexical.Lexemes
{
    internal sealed class SyntaxCharacterLexeme : Lexeme
    {
        public SyntaxCharacterLexeme(SyntaxCharacter syntaxCharacter, int lineNumber, int index) : base(lineNumber, index)
        {
            SyntaxCharacter = syntaxCharacter;
        }

        public override string Content => SyntaxCharacter switch
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
            _ => throw new InvalidOperationException(),
        };

        public SyntaxCharacter SyntaxCharacter { get; }
    }
}
