using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class XorKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public XorKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Xor";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.LogicalXor;
    }
}
