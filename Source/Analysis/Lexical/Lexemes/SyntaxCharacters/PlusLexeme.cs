using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class PlusLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public PlusLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "+";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Additive;
    }
}
