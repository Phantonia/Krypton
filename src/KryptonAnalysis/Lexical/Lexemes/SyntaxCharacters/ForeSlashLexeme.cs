using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ForeSlashLexeme : OperatorLexeme
    {
        public ForeSlashLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "/";

        public override OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.RealDivision;
    }
}
