using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DoubleAsteriskLexeme : OperatorLexeme
    {
        public DoubleAsteriskLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "**";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Exponantiation;
    }
}
