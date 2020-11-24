namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class MinusLexeme : SyntaxCharacterLexeme
    {
        public MinusLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "-";
    }
}
