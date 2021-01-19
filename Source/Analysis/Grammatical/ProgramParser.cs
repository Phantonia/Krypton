using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Grammatical
{
    public sealed class ProgramParser
    {
        public ProgramParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;

            expressionParser = new ExpressionParser(lexemes);
            typeParser = new TypeParser(lexemes);
            statementParser = new StatementParser(lexemes, expressionParser, typeParser);
        }

        private readonly ExpressionParser expressionParser;
        private int index;
        private readonly StatementParser statementParser;
        private readonly TypeParser typeParser;

        public LexemeCollection Lexemes { get; }

        public SyntaxTree? ParseWholeProgram()
        {
            List<StatementNode> statements = new();

            while (TryParseNextNode(out Node? node))
            {
                switch (node)
                {
                    case StatementNode statement:
                        statements.Add(statement);
                        break;
                    default: throw new NotImplementedException("Should not happen");
                }
            }

            StatementCollectionNode topLevelStatements = new(statements);
            ProgramNode programNode = new(topLevelStatements, topLevelStatements.LineNumber);
            return new SyntaxTree(programNode);
        }

        private bool TryParseNextNode([NotNullWhen(true)] out Node? node)
        {
            StatementNode? statementNode = statementParser.ParseNextStatement(ref index);

            if (statementNode != null)
            {
                node = statementNode;
                return true;
            }

            node = null;
            return false;
        }
    }
}
