using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Syntax
{
    partial class StatementParser
    {
        private ExpressionStatementNode? ParseExpressionStatement(ref int index)
        {
            ExpressionNode? expression = expressionParser.ParseNextExpression(ref index);

            if (expression == null)
            {
                return null;
            }

            return expression switch
            {
                InvocationExpressionNode invocation => ParseFunctionCallStatement(invocation, ref index),
                IdentifierExpressionNode identifierExpression => ParseVariableAssignmentStatement(ref index, identifierExpression.IdentifierNode, expression),
                _ => Error(),
            };

            StatementNode? Error()
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.OnlyFunctionCallExpressionCanBeStatement, code, expression);
                //return null;
            }
        }

        private ExpressionStatementNode? ParseFunctionCallStatement(InvocationExpressionNode expression, ref int index)
        {
            if (tokens[index] is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
            {
                index++;
                return new ExpressionStatementNode(expression, semicolon);
            }
            else
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, tokens[index]);
                //return null;
            }
        }

        private LoopControlStatementNode? ParseLoopControlStatement(ref int index, ReservedKeywordToken leaveOrContinue)
        {
            Debug.Assert(leaveOrContinue.Keyword is ReservedKeyword.Leave or ReservedKeyword.Continue);

            index++;

            LiteralToken<long>? level = null;

            if (tokens[index] is LiteralToken<long> { Value: > 0 and < 50 } levelToken)
            {
                level = levelToken;
                index++;
            }

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, tokens[index]);
                //return null;
            }

            index++;

            return new LoopControlStatementNode(leaveOrContinue, level, semicolon);
        }

        private ReturnStatementNode? ParseReturnStatement(ref int index, ReservedKeywordToken returnKeyword)
        {
            Debug.Assert(returnKeyword.Keyword == ReservedKeyword.Return);

            index++;

            if (tokens[index] is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon0)
            {
                index++;

                return new ReturnStatementNode(returnKeyword, returnedExpression: null, semicolon0);
            }

            ExpressionNode? returnExpression = expressionParser.ParseNextExpression(ref index);

            if (returnExpression == null)
            {
                return null;
            }

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon1)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, tokens[index]);
                //return null;
            }

            index++;

            return new ReturnStatementNode(returnKeyword, returnExpression, semicolon1);
        }

        private AssignmentStatementNode? ParseVariableAssignmentStatement(ref int index, IdentifierToken identifier, ExpressionNode expression)
        {
            IdentifierExpressionNode assignedExpression = new(identifier);

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equals)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.OnlyFunctionCallExpressionCanBeStatement, code, expression);
                //return null;
            }

            index++;

            ExpressionNode? newValue = expressionParser.ParseNextExpression(ref index);

            if (newValue == null)
            {
                return null;
            }

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, tokens[index]);
                //return null;
            }

            index++;
            return new AssignmentStatementNode(assignedExpression, equals, newValue, semicolon);
        }

        private VariableDeclarationStatementNode? ParseVariableDeclarationStatement(ref int index, ReservedKeywordToken declarator)
        {
            Debug.Assert(declarator.Keyword is ReservedKeyword.Var or ReservedKeyword.Let);
            bool isReadOnly = declarator.Keyword == ReservedKeyword.Let;

            index++;

            if (tokens[index] is not IdentifierToken identifier)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, current ?? tokens[^1]);
                //return null;
            }

            index++;

            if (tokens[index] is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equals)
            {
                return HandleInitialValue(asClause: null, ref index);
            }

            // there is no value, but the variable is readonly which is not allowed
            if (isReadOnly)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.LetVariableAndConstMustBeInitialized,
                //                          code,
                //                          current ?? tokens[^1]);
                //return null;
            }

            if (tokens[index] is not ReservedKeywordToken { Keyword: ReservedKeyword.As } asKeyword)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedAsOrEquals, code, current ?? tokens[^1]);
                //return null;
            }

            index++;

            TypeNode? type = typeParser.ParseNextType(ref index);

            if (type == null)
            {
                return null;
            }

            AsClauseNode asClause = new(asKeyword, type);

            if (tokens[index] is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equals1)
            {
                equals = equals1;
                return HandleInitialValue(asClause, ref index);
            }

            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedEqualsOrSemicolon, code, current ?? tokens[^1]);
                //return null;
            }

            index++;
            return new VariableDeclarationStatementNode(declarator, identifier, asClause, equals: null, initialValue: null, semicolon);

            VariableDeclarationStatementNode? HandleInitialValue(AsClauseNode? asClause, ref int index)
            {
                index++;

                ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

                if (assignedValue == null)
                {
                    return null;
                }

                if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
                {
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, current ?? tokens[^1]);
                    //return null;
                }

                index++;
                return new VariableDeclarationStatementNode(declarator,
                                                            identifier,
                                                            asClause,
                                                            equals,
                                                            assignedValue,
                                                            semicolon);
            }
        }
    }
}
