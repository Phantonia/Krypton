using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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

        private ForStatementNode? ParseForStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            if (!ParseForVariableSpecification(ref index,
                                               out IdentifierNode? identifier,
                                               out ExpressionNode? initialValue,
                                               out bool declaresNew))
            {
                return null;
            }

            ExpressionNode? condition = null;

            if (lexemes.TryGet(index) is KeywordLexeme { Keyword: ReservedKeyword.While })
            {
                condition = ParseForCondition(ref index, identifier);

                if (condition == null)
                {
                    return null;
                }
            }

            ExpressionNode? withExpression = null;

            if (lexemes.TryGet(index) is KeywordLexeme { Keyword: ReservedKeyword.With })
            {
                withExpression = ParseForWithExpression(ref index, identifier);

                if (withExpression == null)
                {
                    return null;
                }
            }

            if (condition == null && withExpression == null)
            {
                ErrorProvider.ReportError(ErrorCode.ForNeitherWhileNorWith,
                                          code,
                                          lineNumber,
                                          nodeIndex);
                return null;
            }

            StatementCollectionNode? statements = ParseStatementBlock(ref index);

            if (statements == null)
            {
                return null;
            }

            return new ForStatementNode(identifier,
                                        declaresNew,
                                        initialValue,
                                        condition,
                                        withExpression,
                                        statements,
                                        lineNumber,
                                        nodeIndex);
        }

        private ExpressionNode? ParseForCondition(ref int index, IdentifierNode identifier)
        {
            index++;

            switch (lexemes.TryGet(index))
            {
                case BooleanLiteralLexeme { Value: true } booleanLiteral:
                    return new BooleanLiteralExpressionNode(true, booleanLiteral.LineNumber, booleanLiteral.Index);
                case IdentifierLexeme
                {
                    LineNumber: int lineNumber,
                    Index: int nodeIndex
                } conditionIdentifier when conditionIdentifier.Content == identifier.Identifier:
                    {
                        index++;

                        if (lexemes.TryGet(index) is not CharacterOperatorLexeme
                            { PrecedenceGroup: OperatorPrecedenceGroup.Comparison, Operator: var @operator })
                        {
                            ErrorProvider.ReportError(ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                                                      code,
                                                      lexemes.TryGet(index) ?? lexemes[^1]);
                            return null;
                        }

                        index++;

                        ExpressionNode? otherSide = expressionParser.ParseNextExpression(ref index);

                        if (otherSide == null)
                        {
                            return null;
                        }

                        IdentifierExpressionNode conditionIdentifierExpression =
                            new IdentifierExpressionNode(identifier.Identifier,
                                                         conditionIdentifier.LineNumber,
                                                         conditionIdentifier.Index);

                        return new BinaryOperationExpressionNode(conditionIdentifierExpression,
                                                                 otherSide,
                                                                 @operator,
                                                                 lineNumber,
                                                                 nodeIndex);
                    }
                case var lexeme:
                    ErrorProvider.ReportError(ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                                              code,
                                              lexeme ?? lexemes[^1]);
                    return null;
            }
        }

        private bool ParseForVariableSpecification(ref int index,
                               [NotNullWhen(true)] out IdentifierNode? variable,
                                                   out ExpressionNode? initialValue,
                                                   out bool declaresNew)
        {
            declaresNew = false;

            if (lexemes.TryGet(index) is KeywordLexeme { Keyword: ReservedKeyword.Var })
            {
                declaresNew = true;

                index++;
            }

            if (lexemes.TryGet(index) is not IdentifierLexeme identifierLexeme)
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, lexemes.TryGet(index) ?? lexemes[^1]);
                variable = null;
                initialValue = null;
                return false;
            }

            variable = new UnboundIdentifierNode(identifierLexeme.Content,
                                                 identifierLexeme.LineNumber,
                                                 identifierLexeme.Index);

            index++;

            initialValue = null;

            if (lexemes.TryGet(index) is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                index++;

                initialValue = expressionParser.ParseNextExpression(ref index);

                if (initialValue == null)
                {
                    return false;
                }
            }
            else // no initial value
            {
                if (declaresNew)
                {
                    ErrorProvider.ReportError(ErrorCode.NewVariableInForWithoutDefaultValue,
                                              code,
                                              variable,
                                              $"Variable: {identifierLexeme.Content}");
                    return false;
                }
            }

            return true;
        }

        private ExpressionNode? ParseForWithExpression(ref int index, IdentifierNode identifier)
        {
            index++;

            if (lexemes.TryGet(index) is not IdentifierLexeme withIdentifierLexeme
                || withIdentifierLexeme.Content != identifier.Identifier)
            {
                ErrorProvider.ReportError(ErrorCode.ForWithPartHasToAssignIterationVariable,
                                          code,
                                          lexemes.TryGet(index) ?? lexemes[^1],
                                          $"Iteration variable: {identifier.Identifier}");
                return null;
            }

            index++;

            if (lexemes.TryGet(index) is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                Debug.Assert(index - 1 >= 0 & index - 1 < lexemes.Count);
                ErrorProvider.ReportError(ErrorCode.ForWithPartHasToAssignIterationVariable,
                                          code,
                                          lexemes[index - 1],
                                          $"Iteration variable: {identifier.Identifier}");
                return null;
            }

            index++;

            return expressionParser.ParseNextExpression(ref index);
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
