namespace Krypton.Analysis.Lexical.Lexemes
{
    internal interface ILexeme
    {
        string Content { get; }

        int Index { get; }

        int LineNumber { get; }
    }
}
