namespace Krypton.Analysis.Lexical.Lexemes
{
    public abstract class LexemeWithoutValue : Lexeme
    {
        protected LexemeWithoutValue(int lineNumber) : base(lineNumber) { }

        protected sealed override void Construct(string value) { }
    }
}
