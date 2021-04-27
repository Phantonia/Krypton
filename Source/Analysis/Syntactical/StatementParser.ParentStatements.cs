using Krypton.Analysis.Errors;
using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Syntactical
{
    partial class StatementParser
    {
        private BlockStatementNode? ParseBlockStatement(ref int index, ReservedKeywordToken blockKeyword)
        {
            index++;

            BodyNode? body = ParseStatementBlock(ref index);

            if (body == null)
            {
                return null;
            }

            return new BlockStatementNode(blockKeyword, body);
        }

        private ForStatementNode? ParseForStatement(ref int index, ReservedKeywordToken forKeyword)
        {
            index++;

            if (!ParseForVariableSpecification(ref index,
                                               out IdentifierNode? identifier,
                                               out ExpressionNode? initialValue,
                                               out bool declaresNew))
            {
                return null;
            }

            ExpressionNode? condition = null;

            if (tokens.TryGet(index) is ReservedKeywordToken { Keyword: ReservedKeyword.While })
            {
                condition = ParseForCondition(ref index, identifier);

                if (condition == null)
                {
                    return null;
                }
            }

            ExpressionNode? withExpression = null;

            if (tokens.TryGet(index) is ReservedKeywordToken { Keyword: ReservedKeyword.With })
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

            BodyNode? statements = ParseStatementBlock(ref index);

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

        private ExpressionNode? ParseForCondition(ref int index, IdentifierToken identifier)
        {
            index++;

            switch (tokens.TryGet(index))
            {
                case LiteralToken<bool> { Value: true } booleanLiteral:
                    index++;
                    return new LiteralExpressionNode<bool>(booleanLiteral);
                case IdentifierToken conditionIdentifier when conditionIdentifier.Text.Span.SequenceEqual(identifier.Text.Span):
                    {
                        index++;

                        if (tokens[index] is not OperatorToken { Operator: Operator.LessThan
                                                                        or Operator.LessThanEquals
                                                                        or Operator.GreaterThan
                                                                        or Operator.GreaterThanEquals
                                                               } @operator)
                        {
                            throw new NotImplementedException();
                            //ErrorProvider.ReportError(ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                            //                          code,
                            //                          tokens.TryGet(index) ?? tokens[^1]);
                            //return null;
                        }

                        index++;

                        ExpressionNode? otherSide = expressionParser.ParseNextExpression(ref index);

                        if (otherSide == null)
                        {
                            return null;
                        }

                        IdentifierExpressionNode conditionIdentifierExpression = new(conditionIdentifier);

                        return new BinaryOperationExpressionNode(conditionIdentifierExpression, @operator, otherSide);
                    }
                case var lexeme:
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                    //                          code,
                    //                          lexeme ?? tokens[^1]);
                    //return null;
            }
        }

        private bool ParseForVariableSpecification(ref int index,
                               [NotNullWhen(true)] out IdentifierNode? variable,
                                                   out ExpressionNode? initialValue,
                                                   out bool declaresNew)
        {
            declaresNew = false;

            if (tokens.TryGet(index) is ReservedKeywordToken { Keyword: ReservedKeyword.Var })
            {
                declaresNew = true;

                index++;
            }

            if (tokens.TryGet(index) is not IdentifierToken identifierToken)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, tokens.TryGet(index) ?? tokens[^1]);
                //variable = null;
                //initialValue = null;
                //return false;
            }

            variable = new UnboundIdentifierNode(identifierToken.Content,
                                                 identifierToken.LineNumber,
                                                 identifierToken.Index);

            index++;

            initialValue = null;

            if (tokens.TryGet(index) is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
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
                                              $"Variable: {identifierToken.Content}");
                    return false;
                }
            }

            return true;
        }

        private ExpressionNode? ParseForWithExpression(ref int index, IdentifierNode identifier)
        {
            index++;

            if (tokens.TryGet(index) is not IdentifierLexeme withIdentifierLexeme
                || withIdentifierLexeme.Content != identifier.Identifier)
            {
                ErrorProvider.ReportError(ErrorCode.ForWithPartHasToAssignIterationVariable,
                                          code,
                                          tokens.TryGet(index) ?? tokens[^1],
                                          $"Iteration variable: {identifier.Identifier}");
                return null;
            }

            index++;

            if (tokens.TryGet(index) is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                Debug.Assert(index - 1 >= 0 & index - 1 < tokens.Count);
                ErrorProvider.ReportError(ErrorCode.ForWithPartHasToAssignIterationVariable,
                                          code,
                                          tokens[index - 1],
                                          $"Iteration variable: {identifier.Identifier}");
                return null;
            }

            index++;

            return expressionParser.ParseNextExpression(ref index);
        }

        private IfStatementNode? ParseIfStatement(ref int index)
        {
            int lineNumber = tokens[index].LineNumber;
            int nodeIndex = tokens[index].Index;

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
                if (tokens.TryGet(index) is not KeywordLexeme { Keyword: ReservedKeyword.Else })
                {
                    return new IfStatementNode(condition,
                                               statements,
                                               elseIfParts: elseIfParts,
                                               elsePart: null,
                                               lineNumber,
                                               nodeIndex);
                }

                int elseLineNumber = tokens[index].LineNumber;
                int elseIndex = tokens[index].Index;

                index++;

                switch (tokens.TryGet(index))
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

        private WhileStatementNode? ParseWhileStatement(ref int index)
        {
            int lineNumber = tokens[index].LineNumber;
            int nodeIndex = tokens[index].Index;

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
