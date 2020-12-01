using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class LessThanLexeme : OperatorLexeme
    {
        public LessThanLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "<";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Comparison;
    }
}
