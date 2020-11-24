namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class MinusEqualsLexeme : SyntaxCharacterLexeme
    {
        public MinusEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "-=";
    }
}
