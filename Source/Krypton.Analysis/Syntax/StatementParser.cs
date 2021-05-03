using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using System.Collections.Generic;

namespace Krypton.Analysis.Syntax
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
                ReservedKeywordToken { Keyword: ReservedKeyword.Block } blockKeyword => ParseBlockStatement(ref index, blockKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.Var } varKeyword => ParseVariableDeclarationStatement(ref index, varKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.Let } letKeyword => ParseVariableDeclarationStatement(ref index, letKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.While } whileKeyword => ParseWhileStatement(ref index, whileKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.If } ifKeyword => ParseIfStatement(ref index, ifKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.For } forKeyword => ParseForStatement(ref index, forKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.Return } returnKeyword => ParseReturnStatement(ref index, returnKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.Continue } continueKeyword => ParseLoopControlStatement(ref index, continueKeyword),
                ReservedKeywordToken { Keyword: ReservedKeyword.Leave } leaveKeyword => ParseLoopControlStatement(ref index, leaveKeyword),
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
