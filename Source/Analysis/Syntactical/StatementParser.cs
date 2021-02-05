using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;

namespace Krypton.Analysis.Syntactical
{
    public sealed partial class StatementParser
    {
        public StatementParser(LexemeCollection lexemes, ExpressionParser expressionParser, TypeParser typeParser)
        {
            this.lexemes = lexemes;
            this.expressionParser = expressionParser;
            this.typeParser = typeParser;
        }

        private readonly ExpressionParser expressionParser;
        private readonly LexemeCollection lexemes;
        private readonly TypeParser typeParser;

        public StatementNode? ParseNextStatement(ref int index)
        {
            return lexemes[index] switch
            {
                KeywordLexeme { Keyword: ReservedKeyword.Block } => ParseBlockStatement(ref index, lexemes[index].LineNumber),
                KeywordLexeme { Keyword: ReservedKeyword.Var } => ParseVariableDeclarationStatement(ref index),
                KeywordLexeme { Keyword: ReservedKeyword.While } => ParseWhileStatement(ref index),
                _ => ParseExpressionStatement(ref index),
            };
        }
    }
}
