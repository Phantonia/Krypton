using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class GreaterThanLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public GreaterThanLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ">";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Comparison;
    }
}
