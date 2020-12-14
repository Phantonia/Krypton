using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class AndKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public AndKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "And";
        
        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.LogicalAnd;
    }
}
