namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AsteriskLexeme : SyntaxCharacterLexeme
    {
        public AsteriskLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "*";
    }
}
