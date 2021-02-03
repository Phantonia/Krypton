using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;

namespace Krypton.Analysis.Syntactical
{
    public sealed class TypeParser
    {
        public TypeParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;
        }

        public LexemeCollection Lexemes { get; }

        public TypeSpecNode? ParseNextType(ref int index)
        {
            if (Lexemes.TryGet(index) is IdentifierLexeme identifierLexeme)
            {
                index++;
                return new IdentifierTypeSpecNode(identifierLexeme.Content, identifierLexeme.LineNumber);
            }

            return null;
        }
    }
}
