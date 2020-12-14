using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class NotKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public NotKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Not";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Multiplicative;
    }
}
