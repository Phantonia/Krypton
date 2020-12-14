using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class LeftKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public LeftKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Left";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Shift;
    }
}
