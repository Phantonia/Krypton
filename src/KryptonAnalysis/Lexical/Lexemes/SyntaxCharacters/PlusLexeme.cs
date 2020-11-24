namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class PlusLexeme : SyntaxCharacterLexeme
    {
        public PlusLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "+";
    }
}
