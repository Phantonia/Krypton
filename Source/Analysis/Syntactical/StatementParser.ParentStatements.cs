using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Utilities;
using System.Collections.Generic;
using System;

namespace Krypton.Analysis.Syntactical
{
    public sealed partial class StatementParser
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

            return new WhileStatementNode(condition, statements, lineNumber, index);
        }
    }
}
