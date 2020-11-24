namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class CommaLexeme : SyntaxCharacterLexeme
    {
        public CommaLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ",";
    }
}
