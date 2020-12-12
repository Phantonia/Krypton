using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class DivKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public DivKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Div";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Multiplicative;
    }
}
