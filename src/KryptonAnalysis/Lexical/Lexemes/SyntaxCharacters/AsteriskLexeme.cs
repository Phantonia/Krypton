using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AsteriskLexeme : OperatorLexeme
    {
        public AsteriskLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "*";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Multiplication;
    }
}
