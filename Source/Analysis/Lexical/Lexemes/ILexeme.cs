namespace Krypton.Analysis.Lexical.Lexemes
{
    public interface ILexeme
    {
        string Content { get; }

        int LineNumber { get; }
    }
}
