using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Utilities;
using System;

namespace Krypton.Analysis.Syntactical
{
    public sealed partial class StatementParser
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
                IdentifierExpressionNode identifierExpression => ParseVariableAssignmentStatement(identifierExpression.IdentifierNode, ref index),
                _ => throw new NotImplementedException("Error 104: Only function call expressions may be used as statements"),
            };
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
                throw new NotImplementedException("Error 101: Semicolon expected");
            }
        }

        private VariableAssignmentStatementNode? ParseVariableAssignmentStatement(IdentifierNode identifier, ref int index)
        {
            if (lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                throw new NotImplementedException("Error ???: Only function call expression may be used as statements");
            }

            index++;

            ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

            if (assignedValue == null)
            {
                return null;
            }

            if (lexemes[index] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                index++;
                return new VariableAssignmentStatementNode(identifier, assignedValue, identifier.LineNumber, identifier.Index);
            }
            else
            {
                throw new NotImplementedException("Error 101: Semicolon expected");
            }
        }

        private VariableDeclarationStatementNode? ParseVariableDeclarationStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;
            int nodeIndex = lexemes[index].Index;

            index++;

            Lexeme? current = lexemes.TryGet(index);

            if (current is IdentifierLexeme identifierLexeme)
            {
                index++;
                current = lexemes.TryGet(index);

                string identifier = identifierLexeme.Content;
                int identifierLineNumber = identifierLexeme.LineNumber;
                int identifierIndex = identifierLexeme.Index;

                if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
                {
                    return HandleAssignedValue(type: null, ref index);
                }
                else if (current is KeywordLexeme { Keyword: ReservedKeyword.As })
                {
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
                    else if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
                    {
                        index++;
                        return new VariableDeclarationStatementNode(new UnboundIdentifierNode(identifier, identifierLineNumber, identifierIndex),
                                                                    type,
                                                                    value: null,
                                                                    lineNumber,
                                                                    nodeIndex);
                    }
                    else
                    {
                        throw new NotImplementedException("Error 103: Equals or semicolon expected");
                    }
                }
                else
                {
                    throw new NotImplementedException("Error 102: As or equals expected");
                }

                VariableDeclarationStatementNode? HandleAssignedValue(TypeSpecNode? type, ref int index)
                {
                    index++;

                    ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

                    if (assignedValue == null)
                    {
                        return null;
                    }

                    current = lexemes.TryGet(index);

                    if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
                    {
                        index++;
                        return new VariableDeclarationStatementNode(new UnboundIdentifierNode(identifier, identifierLineNumber, identifierIndex),
                                                                    type,
                                                                    assignedValue,
                                                                    lineNumber,
                                                                    nodeIndex);
                    }
                    else
                    {
                        throw new NotImplementedException("Error 101: Semicolon expected");
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Error 105: Identifier expected");
            }
        }
    }
}
