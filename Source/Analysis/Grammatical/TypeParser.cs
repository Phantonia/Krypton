using Krypton.Analysis.AST.TypeSpecs;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;

namespace Krypton.Analysis.Grammatical
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
            if (Lexemes.TryGet(index) is IdentifierLexeme idl)
            {
                index++;
                return new IdentifierTypeSpecNode(idl.Content, idl.LineNumber);
            }

            return null;
        }
    }
}
