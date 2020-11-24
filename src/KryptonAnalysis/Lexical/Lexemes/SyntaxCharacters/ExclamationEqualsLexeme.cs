using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ExclamationEqualsLexeme : OperatorLexeme
    {
        public ExclamationEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "!=";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Equality;
    }
}
