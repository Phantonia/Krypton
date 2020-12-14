using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DoubleAsteriskLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public DoubleAsteriskLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "**"; public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Multiplicative;
    }
}
