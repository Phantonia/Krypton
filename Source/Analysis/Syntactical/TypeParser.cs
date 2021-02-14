using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;

namespace Krypton.Analysis.Syntactical
{
    public sealed class TypeParser
    {
        public TypeParser(LexemeCollection lexemes, string code)
        {
            Lexemes = lexemes;
            this.code = code;
        }

        private readonly string code;

        public LexemeCollection Lexemes { get; }

        public TypeSpecNode? ParseNextType(ref int index)
        {
            if (Lexemes.TryGet(index) is not IdentifierLexeme identifierLexeme)
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier,
                                          code,
                                          Lexemes[index]);
                return null;
            }

            index++;
            return new IdentifierTypeSpecNode(identifierLexeme.Content, identifierLexeme.LineNumber, identifierLexeme.Index);
        }
    }
}
