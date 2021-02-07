using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Syntactical
{
    partial class StatementParser
    {
        private BlockStatementNode? ParseBlockStatement(ref int index, int lineNumber)
        {
            int nodeIndex = lexemes[index].Index;

            index++;

            StatementCollectionNode? statements = ParseStatementBlock(ref index);

            if (statements == null)
            {
                return null;
            }

            return new BlockStatementNode(statements, lineNumber, nodeIndex);
        }

        private IfStatementNode? ParseIfStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            ExpressionNode? condition = expressionParser.ParseNextExpression(ref index);

            if (condition == null)
            {
                return null;
            }

            StatementCollectionNode? statements = ParseStatementBlock(ref index);

            if (statements == null)
            {
                return null;
            }

            List<ElseIfPartNode>? elseIfParts = null;

            while (true)
            {
                if (lexemes.TryGet(index) is not KeywordLexeme { Keyword: ReservedKeyword.Else })
                {
                    return new IfStatementNode(condition,
                                               statements,
                                               elseIfParts: elseIfParts,
                                               elsePart: null,
                                               lineNumber,
                                               nodeIndex);
                }

                int elseLineNumber = lexemes[index].LineNumber;
                int elseIndex = lexemes[index].Index;

                index++;

                switch (lexemes.TryGet(index))
                {
                    case KeywordLexeme { Keyword: ReservedKeyword.If }:
                        {
                            index++;

                            ExpressionNode? elseIfCondition
                                = expressionParser.ParseNextExpression(ref index);

                            if (elseIfCondition == null)
                            {
                                return null;
                            }

                            StatementCollectionNode? elseIfStatements
                                = ParseStatementBlock(ref index);

                            if (elseIfStatements == null)
                            {
                                return null;
                            }

                            elseIfParts ??= new List<ElseIfPartNode>();
                            elseIfParts.Add(new ElseIfPartNode(elseIfCondition, elseIfStatements, elseLineNumber, elseIndex));
                        }
                        break;
                    default:
                        {
                            StatementCollectionNode? elseStatements = ParseStatementBlock(ref index);

                            if (elseStatements == null)
                            {
                                return null;
                            }

                            return new IfStatementNode(condition,
                                                       statements,
                                                       elseIfParts,
                                                       new ElsePartNode(elseStatements, elseLineNumber, elseIndex),
                                                       lineNumber,
                                                       nodeIndex);
                        }
                }
            }
        }

        private StatementCollectionNode? ParseStatementBlock(ref int index)
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

        private WhileStatementNode? ParseWhileStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            ExpressionNode? condition = expressionParser.ParseNextExpression(ref index);

            if (condition == null)
            {
                return null;
            }

            StatementCollectionNode? statements = ParseStatementBlock(ref index);

            if (statements == null)
            {
                return null;
            }

            return new WhileStatementNode(condition, statements, lineNumber, nodeIndex);
        }
    }
}
