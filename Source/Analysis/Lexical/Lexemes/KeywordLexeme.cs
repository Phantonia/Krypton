using Krypton.Analysis.Syntactical;

namespace Krypton.Analysis.Lexical.Lexemes
{
    internal class KeywordLexeme : Lexeme
    {
        private KeywordLexeme(ReservedKeyword keyword, int lineNumber, int index) : base(lineNumber, index)
        {
            Keyword = keyword;
        }

        public override string Content => Keyword.ToString();

        public ReservedKeyword Keyword { get; }

        public static KeywordLexeme Create(ReservedKeyword keyword, int lineNumber, int index)
        {
            return keyword switch
            {
                ReservedKeyword.And => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalAnd, keyword, lineNumber, index),
                ReservedKeyword.Or => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalOr, keyword, lineNumber, index),
                ReservedKeyword.Xor => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalXor, keyword, lineNumber, index),
                ReservedKeyword.Not => new OperatorKeywordLexeme(OperatorPrecedenceGroup.LogicalNot, keyword, lineNumber, index),
                ReservedKeyword.Div or ReservedKeyword.Mod => new OperatorKeywordLexeme(OperatorPrecedenceGroup.Multiplicative, keyword, lineNumber, index),
                _ => new KeywordLexeme(keyword, lineNumber, index),
            };
        }

        private sealed class OperatorKeywordLexeme : KeywordLexeme, IOperatorLexeme
        {
            public OperatorKeywordLexeme(OperatorPrecedenceGroup precedenceGroup, ReservedKeyword keyword, int lineNumber, int index) : base(keyword, lineNumber, index)
            {
                PrecedenceGroup = precedenceGroup;
            }

            public OperatorPrecedenceGroup PrecedenceGroup { get; }
        }
    }
}
