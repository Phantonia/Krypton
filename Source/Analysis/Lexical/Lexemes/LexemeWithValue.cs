namespace Krypton.Analysis.Lexical.Lexemes
{
    public abstract class LexemeWithValue : Lexeme
    {
        protected LexemeWithValue(int lineNumber) : base(lineNumber) { }

        protected sealed override void Construct() { }

        protected abstract override void Construct(string value);
    }
}
