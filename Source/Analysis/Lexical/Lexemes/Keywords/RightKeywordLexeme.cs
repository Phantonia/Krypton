using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class RightKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public RightKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Right";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Shift;
    }
}
