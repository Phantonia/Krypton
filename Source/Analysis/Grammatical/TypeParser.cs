﻿using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;

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
