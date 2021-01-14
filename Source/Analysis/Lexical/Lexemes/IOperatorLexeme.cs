using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes
{
    // This interface is intended only to be implemented by classes that inherited from
    // Krypton.Analysis.Lexical.Lexemes.Lexeme. It can't be a class, because both
    // KeywordLexemes and SyntaxCharacterLexemes can be binary operators.
    public interface IOperatorLexeme : ILexeme
    {
        OperatorPrecedenceGroup PrecedenceGroup { get; }
    }
}
