﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System;

namespace Krypton.Analysis.Grammatical
{
    public sealed class StatementParser
    {
        public StatementParser(LexemeCollection lexemes, ExpressionParser expressionParser, TypeParser typeParser)
        {
            this.lexemes = lexemes;
            this.expressionParser = expressionParser;
            this.typeParser = typeParser;
        }

        private readonly ExpressionParser expressionParser;
        private readonly LexemeCollection lexemes;
        private readonly TypeParser typeParser;

        public StatementNode? ParseNextStatement(ref int index)
        {
            return lexemes[index] switch
            {
                VarKeywordLexeme => ParseVariableDeclarationStatement(ref index),
                _ => ParseExpressionStatement(ref index),
            };
        }

        private StatementNode? ParseExpressionStatement(ref int index)
        {
            ExpressionNode? expression = expressionParser.ParseNextExpression(ref index);

            if (expression == null)
            {
                return null;
            }

            return expression switch
            {
                FunctionCallExpressionNode fcen => ParseFunctionCallStatement(fcen, ref index),
                IdentifierExpressionNode iden => ParseVariableAssignmentStatement(iden.Identifier, ref index),
                _ => throw new NotImplementedException("Error 104: Only function call expressions may be used as statements"),
            };
        }

        private FunctionCallStatementNode? ParseFunctionCallStatement(FunctionCallExpressionNode expression, ref int index)
        {
            if (lexemes[index] is SemicolonLexeme)
            {
                index++;
                return new FunctionCallStatementNode(expression, expression.LineNumber);
            }
            else
            {
                throw new NotImplementedException("Error 101: Semicolon expected");
            }
        }

        private VariableAssignmentStatementNode? ParseVariableAssignmentStatement(IdentifierNode identifier, ref int index)
        {
            if (lexemes[index] is not EqualsLexeme)
            {
                throw new NotImplementedException("Error ???: Only function call expression may be used as statements");
            }

            index++;

            ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

            if (assignedValue == null)
            {
                return null;
            }

            if (lexemes[index] is SemicolonLexeme)
            {
                return new VariableAssignmentStatementNode(identifier, assignedValue, identifier.LineNumber);
            }
            else
            {
                throw new NotImplementedException("Error 101: Semicolon expected");
            }
        }

        private VariableDeclarationStatementNode? ParseVariableDeclarationStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;

            index++;
            Lexeme? current = lexemes.TryGet(index);

            if (current is IdentifierLexeme idl)
            {
                index++;
                current = lexemes.TryGet(index);

                string variableName = idl.Content;

                if (current is EqualsLexeme)
                {
                    return HandleAssignedValue(type: null, ref index);
                }
                else if (current is AsKeywordLexeme)
                {
                    index++;

                    TypeNode? type = typeParser.ParseNextType(ref index);

                    if (type == null)
                    {
                        return null;
                    }

                    current = lexemes.TryGet(index);

                    if (current is EqualsLexeme)
                    {
                        return HandleAssignedValue(type, ref index);
                    }
                    else if (current is SemicolonLexeme)
                    {
                        return new VariableDeclarationStatementNode(variableName, type, value: null, lineNumber);
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

                VariableDeclarationStatementNode? HandleAssignedValue(TypeNode? type, ref int index)
                {
                    index++;

                    ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

                    if (assignedValue == null)
                    {
                        return null;
                    }

                    current = lexemes.TryGet(index);

                    if (current is SemicolonLexeme)
                    {
                        return new VariableDeclarationStatementNode(variableName, type, assignedValue, lineNumber);
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