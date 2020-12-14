using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class ModKeywordLexeme : KeywordLexeme, IOperatorLexeme
    {
        public ModKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Mod";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Multiplicative;
    }
}
