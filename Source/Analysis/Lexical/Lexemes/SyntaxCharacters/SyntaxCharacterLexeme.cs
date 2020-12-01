using Krypton.Analysis.Lexical.Lexemes;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public abstract class SyntaxCharacterLexeme : LexemeWithoutValue
    {
        protected SyntaxCharacterLexeme(int lineNumber) : base(lineNumber) { }
    }
}
