using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using System.Collections.Generic;

namespace Krypton.Analysis.Syntactical
{
    internal sealed partial class StatementParser
    {
        public StatementParser(FinalList<Token> tokens,
                               ExpressionParser expressionParser,
                               TypeParser typeParser,
                               string code)
        {
            this.tokens = tokens;
            this.expressionParser = expressionParser;
            this.typeParser = typeParser;
            this.code = code;
        }

        private readonly string code;
        private readonly ExpressionParser expressionParser;
        private readonly FinalList<Token> tokens;
        private readonly TypeParser typeParser;

        public StatementNode? ParseNextStatement(ref int index)
        {
            return tokens[index] switch
            {
                ReservedKeywordToken { Keyword: ReservedKeyword.Block } => ParseBlockStatement(ref index, tokens[index].LineNumber),
                ReservedKeywordToken { Keyword: ReservedKeyword.Var } => ParseVariableDeclarationStatement(ref index, isReadOnly: false),
                ReservedKeywordToken { Keyword: ReservedKeyword.Let } => ParseVariableDeclarationStatement(ref index, isReadOnly: true),
                ReservedKeywordToken { Keyword: ReservedKeyword.While } => ParseWhileStatement(ref index),
                ReservedKeywordToken { Keyword: ReservedKeyword.If } => ParseIfStatement(ref index),
                ReservedKeywordToken { Keyword: ReservedKeyword.For } => ParseForStatement(ref index),
                ReservedKeywordToken { Keyword: ReservedKeyword.Return } => ParseReturnStatement(ref index),
                ReservedKeywordToken { Keyword: ReservedKeyword.Continue } => ParseLoopControlStatement(ref index, LoopControlStatementKind.Continue),
                ReservedKeywordToken { Keyword: ReservedKeyword.Leave } => ParseLoopControlStatement(ref index, LoopControlStatementKind.Leave),
                _ => ParseExpressionStatement(ref index),
            };
        }

        public BodyNode? ParseStatementBlock(ref int index)
        {
            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.BraceOpening } openingBrace)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedOpeningBrace, code, tokens[index]);
                //return null;
            }

            index++;

            List<StatementNode> statements = new();

            while (true)
            {
                switch (tokens.TryGet(index))
                {
                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.BraceClosing } closingBrace:
                        index++;
                        return new BodyNode(openingBrace, statements, closingBrace);
                    case null:
                        throw new NotImplementedException();
                        //ErrorProvider.ReportError(ErrorCode.ExpectedClosingBrace, code, tokens[^1]);
                        //return null;
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
