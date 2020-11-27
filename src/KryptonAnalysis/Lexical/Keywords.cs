using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using System;
using System.Collections.Immutable;

namespace Krypton.Analysis.Lexical
{
    public static class Keywords
    {
        static Keywords()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Func<int, Lexeme>>();
            AddKeyword<AndKeywordLexeme>(builder, "And");
            AddKeyword<AsKeywordLexeme>(builder, "As");
            AddKeyword<DivKeywordLexeme>(builder, "Div");
            AddKeyword<ModKeywordLexeme>(builder, "Mod");
            AddKeyword<NotKeywordLexeme>(builder, "Not");
            AddKeyword<OrKeywordLexeme>(builder, "Or");
            AddKeyword<VarKeywordLexeme>(builder, "Var");
            AddKeyword<OutKeywordLexeme>(builder, "Out");
            builder.Add("True", lineNumber => new BooleanLiteralLexeme(true, lineNumber));
            builder.Add("False", lineNumber => new BooleanLiteralLexeme(false, lineNumber));
            ReservedKeywords = builder.ToImmutable();
        }

        public static ImmutableDictionary<string, Func<int, Lexeme>> ReservedKeywords { get; }

        private static void AddKeyword<TLexeme>(ImmutableDictionary<string, Func<int, Lexeme>>.Builder builder, string keyword)
            where TLexeme : KeywordLexeme
        {
            builder.Add(keyword, Lexeme.New<TLexeme>);
            //builder.Add(keyword, lineNumber => Lexeme.New<TLexeme>(lineNumber));
        }
    }
}
