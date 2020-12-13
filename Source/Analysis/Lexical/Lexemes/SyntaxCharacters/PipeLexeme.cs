using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class PipeLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public PipeLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "|";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Bitwise;
    }
}
