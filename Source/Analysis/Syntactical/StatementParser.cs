using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Syntactical
{
    internal sealed partial class StatementParser
    {
        public StatementParser(LexemeCollection lexemes,
                               ExpressionParser expressionParser,
                               TypeParser typeParser,
                               string code)
        {
            this.lexemes = lexemes;
            this.expressionParser = expressionParser;
            this.typeParser = typeParser;
            this.code = code;
        }

        private readonly string code;
        private readonly ExpressionParser expressionParser;
        private readonly LexemeCollection lexemes;
        private readonly TypeParser typeParser;

        public StatementNode? ParseNextStatement(ref int index)
        {
            return lexemes[index] switch
            {
                KeywordLexeme { Keyword: ReservedKeyword.Block } => ParseBlockStatement(ref index, lexemes[index].LineNumber),
                KeywordLexeme { Keyword: ReservedKeyword.Var } => ParseVariableDeclarationStatement(ref index, isReadOnly: false),
                KeywordLexeme { Keyword: ReservedKeyword.Let } => ParseVariableDeclarationStatement(ref index, isReadOnly: true),
                KeywordLexeme { Keyword: ReservedKeyword.While } => ParseWhileStatement(ref index),
                KeywordLexeme { Keyword: ReservedKeyword.If } => ParseIfStatement(ref index),
                KeywordLexeme { Keyword: ReservedKeyword.For } => ParseForStatement(ref index),
                KeywordLexeme { Keyword: ReservedKeyword.Return } => ParseReturnStatement(ref index),
                KeywordLexeme { Keyword: ReservedKeyword.Continue } => ParseLoopControlStatement(ref index, LoopControlStatementKind.Continue),
                KeywordLexeme { Keyword: ReservedKeyword.Leave } => ParseLoopControlStatement(ref index, LoopControlStatementKind.Leave),
                _ => ParseExpressionStatement(ref index),
            };
        }

        public StatementCollectionNode? ParseStatementBlock(ref int index)
        {
            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.BraceOpening })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedOpeningBrace, code, lexemes[index]);
                return null;
            }

            index++;

            List<StatementNode> statements = new();

            while (true)
            {
                switch (lexemes.TryGet(index))
                {
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.BraceClosing }:
                        index++;
                        return new StatementCollectionNode(statements);
                    case null:
                        ErrorProvider.ReportError(ErrorCode.ExpectedClosingBrace, code, lexemes[^1]);
                        return null;
                }

                StatementNode? nextStatement = ParseNextStatement(ref index);

                if (nextStatement == null)
                {
                    return null;
                }

                statements.Add(nextStatement);
            }
        }
    }
}
