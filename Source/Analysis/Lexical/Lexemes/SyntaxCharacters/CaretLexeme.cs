using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class CaretLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public CaretLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "^";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Bitwise;
    }
}
