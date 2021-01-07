using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System;
using System.Collections.Generic;

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
                KeywordLexeme { Keyword: ReservedKeyword.Block } => ParseBlockStatement(ref index, lexemes[index].LineNumber),
                KeywordLexeme { Keyword: ReservedKeyword.Var } => ParseVariableDeclarationStatement(ref Increase(ref index)),
                KeywordLexeme { Keyword: ReservedKeyword.While } => ParseWhileStatement(ref index),
                _ => ParseExpressionStatement(ref index),
            };

            static ref int Increase(ref int index)
            {
                index++;
                return ref index;
            }
        }

        private BlockStatementNode? ParseBlockStatement(ref int index, int lineNumber)
        {
            index++;

            StatementCollectionNode? statements = ParseStatementBlock(ref index);

            if (statements == null)
            {
                return null;
            }

            return new BlockStatementNode(statements, lineNumber);
        }

        private StatementCollectionNode? ParseStatementBlock(ref int index)
        {
            if (lexemes[index] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.BraceOpening })
            {
                index++;

                List<StatementNode> statements = new();

                while (true)
                {
                    switch (lexemes.TryGet(index))
                    {
                        case null:
                            throw new NotImplementedException("Error ???: closing brace expected");
                        case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.BraceClosing }:
                            index++;
                            return new StatementCollectionNode(statements);
                    }

                    StatementNode? nextStatement = ParseNextStatement(ref index);

                    if (nextStatement == null)
                    {
                        return null;
                    }

                    statements.Add(nextStatement);
                }
            }
            else
            {
                throw new NotImplementedException("Error ???: opening brace expected");
            }
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
                IdentifierExpressionNode iden => ParseVariableAssignmentStatement(iden.IdentifierNode, ref index),
                _ => throw new NotImplementedException("Error 104: Only function call expressions may be used as statements"),
            };
        }

        private FunctionCallStatementNode? ParseFunctionCallStatement(FunctionCallExpressionNode expression, ref int index)
        {
            if (lexemes[index] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
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

            //index++;
            Lexeme? current = lexemes.TryGet(index);

            if (current is IdentifierLexeme idl)
            {
                index++;
                current = lexemes.TryGet(index);

                string identifier = idl.Content;
                int identifierLineNumber = idl.LineNumber;

                if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
                {
                    return HandleAssignedValue(type: null, ref index);
                }
                else if (current is KeywordLexeme { Keyword: ReservedKeyword.As })
                {
                    index++;

                    TypeNode? type = typeParser.ParseNextType(ref index);

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
                        return new VariableDeclarationStatementNode(new IdentifierNode(identifier, identifierLineNumber), type, value: null, lineNumber);
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

                    if (current is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
                    {
                        index++;
                        return new VariableDeclarationStatementNode(new IdentifierNode(identifier, identifierLineNumber), type, assignedValue, lineNumber);
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

        private WhileStatementNode? ParseWhileStatement(ref int index)
        {
            int lineNumber = lexemes[index].LineNumber;

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

            return new WhileStatementNode(condition, statements, lineNumber);
        }
    }
}
