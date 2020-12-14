using Krypton.Analysis.Grammatical;
namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class OrKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public OrKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Or";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.LogicalOr;
    }
}
