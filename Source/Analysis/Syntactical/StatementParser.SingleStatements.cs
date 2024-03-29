﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;

namespace Krypton.Analysis.Syntactical
{
    partial class StatementParser
    {
        private StatementNode? ParseExpressionStatement(ref int index)
        {
            ExpressionNode? expression = expressionParser.ParseNextExpression(ref index);

            if (expression == null)
            {
                return null;
            }

            return expression switch
            {
                FunctionCallExpressionNode functionCall => ParseFunctionCallStatement(functionCall, ref index),
                IdentifierExpressionNode identifierExpression => ParseVariableAssignmentStatement(ref index, identifierExpression.IdentifierNode, expression),
                _ => Error(),
            };

            StatementNode? Error()
            {
                ErrorProvider.ReportError(ErrorCode.OnlyFunctionCallExpressionCanBeStatement, code, expression);
                return null;
            }
        }

        private FunctionCallStatementNode? ParseFunctionCallStatement(FunctionCallExpressionNode expression, ref int index)
        {
            if (lexemes[index] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                index++;
                return new FunctionCallStatementNode(expression, expression.LineNumber, expression.Index);
            }
            else
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, lexemes[index]);
                return null;
            }
        }

        private LoopControlStatementNode? ParseLoopControlStatement(ref int index, LoopControlStatementKind kind)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            if (lexemes[index] is not IntegerLiteralLexeme { Value: > 0 and < 50 and long level })
            {
                level = 1;
            }
            else
            {
                index++;
            }

            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, lexemes[index]);
                return null;
            }

            index++;

            return new LoopControlStatementNode((ushort)level, kind, lineNumber, nodeIndex);
        }

        private ReturnStatementNode? ParseReturnStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            if (lexemes[index] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                index++;

                return new ReturnStatementNode(lineNumber, nodeIndex);
            }

            ExpressionNode? returnExpression = expressionParser.ParseNextExpression(ref index);

            if (returnExpression == null)
            {
                return null;
            }

            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, lexemes[index]);
                return null;
            }

            index++;

            return new ReturnStatementNode(returnExpression, lineNumber, nodeIndex);
        }

        private VariableAssignmentStatementNode? ParseVariableAssignmentStatement(ref int index, IdentifierNode identifier, ExpressionNode expression)
        {
            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                ErrorProvider.ReportError(ErrorCode.OnlyFunctionCallExpressionCanBeStatement, code, expression);
                return null;
            }

            index++;

            ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

            if (assignedValue == null)
            {
                return null;
            }

            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, lexemes[index]);
                return null;
            }

            index++;
            return new VariableAssignmentStatementNode(identifier, assignedValue, identifier.LineNumber, identifier.Index);
        }

        private VariableDeclarationStatementNode? ParseVariableDeclarationStatement(ref int index, bool isReadOnly)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            Lexeme? current = lexemes.TryGet(index);

            if (current is not IdentifierLexeme identifierLexeme)
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, current ?? lexemes[^1]);
                return null;
            }

            index++;
            current = lexemes.TryGet(index);

            string identifier = identifierLexeme.Content;
            int identifierLineNumber = identifierLexeme.LineNumber;
            int identifierIndex = identifierLexeme.Index;

            if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                return HandleAssignedValue(type: null, ref index);
            }

            if (isReadOnly)
            {
                ErrorProvider.ReportError(ErrorCode.LetVariableAndConstMustBeInitialized,
                                          code,
                                          current ?? lexemes[^1]);
                return null;
            }

            if (current is not KeywordLexeme { Keyword: ReservedKeyword.As })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedAsOrEquals, code, current ?? lexemes[^1]);
                return null;
            }

            index++;

            TypeSpecNode? type = typeParser.ParseNextType(ref index);

            if (type == null)
            {
                return null;
            }

            current = lexemes.TryGet(index);

            if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                return HandleAssignedValue(type, ref index);
            }

            if (current is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedEqualsOrSemicolon, code, current ?? lexemes[^1]);
                return null;
            }

            index++;
            return new VariableDeclarationStatementNode(new UnboundIdentifierNode(identifier, identifierLineNumber, identifierIndex),
                                                        type,
                                                        value: null,
                                                        isReadOnly,
                                                        lineNumber,
                                                        nodeIndex);

            VariableDeclarationStatementNode? HandleAssignedValue(TypeSpecNode? type, ref int index)
            {
                index++;

                ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

                if (assignedValue == null)
                {
                    return null;
                }

                current = lexemes.TryGet(index);

                if (current is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
                {
                    ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon, code, current ?? lexemes[^1]);
                    return null;
                }

                index++;
                return new VariableDeclarationStatementNode(new UnboundIdentifierNode(identifier, identifierLineNumber, identifierIndex),
                                                            type,
                                                            assignedValue,
                                                            isReadOnly,
                                                            lineNumber,
                                                            nodeIndex);
            }
        }
    }
}
