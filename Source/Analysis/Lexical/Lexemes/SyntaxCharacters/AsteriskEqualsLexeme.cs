using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AsteriskEqualsLexeme : SyntaxCharacterLexeme
    {
        public AsteriskEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "*=";
    }
}
