namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ExclamationEqualsLexeme : SyntaxCharacterLexeme
    {
        public ExclamationEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "!=";
    }
}
