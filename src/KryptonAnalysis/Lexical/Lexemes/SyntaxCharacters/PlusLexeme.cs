using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class PlusLexeme : OperatorLexeme
    {
        public PlusLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "+";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Additive;
    }
}
