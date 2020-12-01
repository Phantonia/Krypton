using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class GreaterThanLexeme : OperatorLexeme
    {
        public GreaterThanLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ">";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Comparison;
    }
}
