using Krypton.Analysis.Syntactical;

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
                ReservedKeyword.And => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalAnd, keyword, lineNumber),
                ReservedKeyword.Or => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalOr, keyword, lineNumber),
                ReservedKeyword.Xor => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalXor, keyword, lineNumber),
                ReservedKeyword.Not => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalNot, keyword, lineNumber),
                ReservedKeyword.Div or ReservedKeyword.Mod => new OperatorKeywordLexeme(OperatorPrecedenceGroup.Multiplicative, keyword, lineNumber),
                _ => new KeywordLexeme(keyword, lineNumber),
            };
        }

        private sealed class OperatorKeywordLexeme : KeywordLexeme, IOperatorLexeme
        {
            public OperatorKeywordLexeme(OperatorPrecedenceGroup precedenceGroup, ReservedKeyword keyword, int lineNumber) : base(keyword, lineNumber)
            {
                PrecedenceGroup = precedenceGroup;
            }

            public OperatorPrecedenceGroup PrecedenceGroup { get; }
        }
    }
}
