using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes
{
    public class KeywordLexeme : Lexeme
    {
        private KeywordLexeme(ReservedKeyword keyword, int lineNumber) : base(lineNumber)
        {
            Keyword = keyword;
        }

        public override string Content => Keyword.ToString();

        public ReservedKeyword Keyword { get; }

        public static KeywordLexeme Create(ReservedKeyword keyword, int lineNumber)
        {
            return keyword switch
            {
                ReservedKeyword.And => new BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalAnd, keyword, lineNumber),
                ReservedKeyword.Or => new BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalOr, keyword, lineNumber),
                ReservedKeyword.Xor => new BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalXor, keyword, lineNumber),
                ReservedKeyword.Not => new BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalNot, keyword, lineNumber),
                ReservedKeyword.Div or ReservedKeyword.Mod => new BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup.Multiplicative, keyword, lineNumber),
                _ => new KeywordLexeme(keyword, lineNumber),
            };
        }

        private sealed class BinaryOperatorKeywordLexeme : KeywordLexeme, IOperatorLexeme
        {
            public BinaryOperatorKeywordLexeme(OperatorPrecedenceGroup precedenceGroup, ReservedKeyword keyword, int lineNumber) : base(keyword, lineNumber)
            {
                PrecedenceGroup = precedenceGroup;
            }

            public OperatorPrecedenceGroup PrecedenceGroup { get; }
        }
    }
}
