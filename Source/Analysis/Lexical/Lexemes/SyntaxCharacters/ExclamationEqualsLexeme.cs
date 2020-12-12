using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ExclamationEqualsLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public ExclamationEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "!=";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Equality;
    }
}
