using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Syntax
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
                                               out ReservedKeywordToken? varKeyword,
                                               out IdentifierToken? variable,
                                               out SyntaxCharacterToken? equals,
                                               out ExpressionNode? initialValue))
            {
                return null;
            }

            ReservedKeywordToken? whileKeyword = null;
            ExpressionNode? condition = null;

            if (tokens[index] is ReservedKeywordToken { Keyword: ReservedKeyword.While } whileKeywordTmp)
            {
                whileKeyword = whileKeywordTmp;
                condition = ParseForCondition(ref index, variable);

                if (condition == null)
                {
                    return null;
                }
            }

            ReservedKeywordToken? withKeyword = null;
            IdentifierToken? withIdentifier = null;
            SyntaxCharacterToken? withEquals = null;
            ExpressionNode? withExpression = null;

            if (tokens[index] is ReservedKeywordToken { Keyword: ReservedKeyword.With } withKeywordTmp)
            {
                withKeyword = withKeywordTmp;
                (withIdentifier, withEquals, withExpression) = ParseForWithExpression(ref index, variable);

                if (withExpression == null)
                {
                    return null;
                }
            }

            if (condition == null && withExpression == null)
            {
                Debugger.Break();
                analyser.ReportError(DiagnosticsCode.ForNeitherWhileNorWith, tokens[index]);
                return null;
            }

            BodyNode? body = ParseStatementBlock(ref index);

            if (body == null)
            {
                return null;
            }

            return new ForStatementNode(forKeyword,
                                        varKeyword,
                                        variable,
                                        equals,
                                        initialValue,
                                        whileKeyword,
                                        condition,
                                        withKeyword,
                                        withIdentifier,
                                        withEquals,
                                        withExpression,
                                        body);
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

                        if (tokens[index] is not OperatorToken
                            {
                                Operator: Operator.LessThan
                                       or Operator.LessThanEquals
                                       or Operator.GreaterThan
                                       or Operator.GreaterThanEquals
                            } @operator)
                        {
                            analyser.ReportError(DiagnosticsCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                                                 tokens.TryGet(index) ?? tokens[^1]);
                            return null;
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
                case var token:
                    analyser.ReportError(DiagnosticsCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable,
                                         token ?? tokens[^1]);
                    return null;
            }
        }

        private bool ParseForVariableSpecification(ref int index,
                                                   out ReservedKeywordToken? varKeyword,
                               [NotNullWhen(true)] out IdentifierToken? variable,
                                                   out SyntaxCharacterToken? equals,
                                                   out ExpressionNode? initialValue)
        {
            if (tokens[index] is ReservedKeywordToken { Keyword: ReservedKeyword.Var } varKeywordTmp)
            {
                varKeyword = varKeywordTmp;

                index++;
            }
            else
            {
                varKeyword = null;
            }

            if (tokens[index] is not IdentifierToken variableTmp)
            {
                analyser.ReportError(DiagnosticsCode.ExpectedIdentifier, tokens.TryGet(index) ?? tokens[^1]);
                variable = null;
                equals = null;
                initialValue = null;
                return false;
            }

            variable = variableTmp;

            index++;

            initialValue = null;

            if (tokens[index] is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equalsTmp)
            {
                equals = equalsTmp;

                index++;

                initialValue = expressionParser.ParseNextExpression(ref index);

                if (initialValue == null)
                {
                    return false;
                }
            }
            else // no initial value
            {
                if (varKeyword != null) // new variable is declared
                {
                    analyser.ReportError(DiagnosticsCode.NewVariableInForWithoutDefaultValue,
                                         tokens[index],
                                         $"Variable: {variableTmp.Text}");
                    equals = null;
                    return false;
                }
                else
                {
                    equals = null;
                    initialValue = null;
                }
            }

            return true;
        }

        private (IdentifierToken? iterationVariable, SyntaxCharacterToken? equals, ExpressionNode? expression) ParseForWithExpression(ref int index, IdentifierToken iterationVariable)
        {
            index++;

            if (tokens[index] is not IdentifierToken withIdentifier
                || !withIdentifier.Text.Span.SequenceEqual(iterationVariable.Text.Span))
            {
                analyser.ReportError(DiagnosticsCode.ForWithPartHasToAssignIterationVariable,
                                     tokens.TryGet(index) ?? tokens[^1],
                                     $"Iteration variable: {iterationVariable.Text}");
                return (null, null, null);
            }

            index++;

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equals)
            {
                Debug.Assert(index - 1 >= 0 & index - 1 < tokens.Count);
                analyser.ReportError(DiagnosticsCode.ForWithPartHasToAssignIterationVariable,
                                     tokens[index],
                                     $"Iteration variable: {iterationVariable.Text}");
                return (null, null, null);
            }

            index++;

            ExpressionNode? expression = expressionParser.ParseNextExpression(ref index);

            return (withIdentifier, equals, expression);
        }

        private IfStatementNode? ParseIfStatement(ref int index, ReservedKeywordToken ifKeyword)
        {
            Debug.Assert(ifKeyword.Keyword is ReservedKeyword.If);

            index++;

            ExpressionNode? condition = expressionParser.ParseNextExpression(ref index);

            if (condition == null)
            {
                return null;
            }

            BodyNode? ifBody = ParseStatementBlock(ref index);

            if (ifBody == null)
            {
                return null;
            }

            List<ElseIfPartNode>? elseIfParts = null;

            while (true)
            {
                if (tokens.TryGet(index) is not ReservedKeywordToken { Keyword: ReservedKeyword.Else } elseKeyword)
                {
                    return new IfStatementNode(ifKeyword, condition, ifBody, elseIfParts, elsePart: null);
                }

                index++;

                switch (tokens.TryGet(index))
                {
                    case ReservedKeywordToken { Keyword: ReservedKeyword.If } ifKeywordElse:
                        {
                            index++;

                            ExpressionNode? elseIfCondition
                                = expressionParser.ParseNextExpression(ref index);

                            if (elseIfCondition == null)
                            {
                                return null;
                            }

                            BodyNode? elseIfBody
                                = ParseStatementBlock(ref index);

                            if (elseIfBody == null)
                            {
                                return null;
                            }

                            elseIfParts ??= new List<ElseIfPartNode>();
                            elseIfParts.Add(new ElseIfPartNode(elseKeyword, ifKeywordElse, condition, elseIfBody));
                        }
                        break;
                    default:
                        {
                            BodyNode? elseBody = ParseStatementBlock(ref index);

                            if (elseBody == null)
                            {
                                return null;
                            }

                            ElsePartNode elsePart = new(elseKeyword, elseBody);

                            return new IfStatementNode(ifKeyword, condition, ifBody, elseIfParts, elsePart);
                        }
                }
            }
        }

        private WhileStatementNode? ParseWhileStatement(ref int index, ReservedKeywordToken whileKeyword)
        {
            index++;

            ExpressionNode? condition = expressionParser.ParseNextExpression(ref index);

            if (condition == null)
            {
                return null;
            }

            BodyNode? body = ParseStatementBlock(ref index);

            if (body == null)
            {
                return null;
            }

            return new WhileStatementNode(whileKeyword, condition, body);
        }
    }
}
