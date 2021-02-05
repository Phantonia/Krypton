using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Syntactical
{
    public sealed class ProgramParser
    {
        public ProgramParser(LexemeCollection lexemes, string code)
        {
            Lexemes = lexemes;

            this.code = code;
            expressionParser = new ExpressionParser(lexemes, code);
            typeParser = new TypeParser(lexemes);
            statementParser = new StatementParser(lexemes, expressionParser, typeParser);
        }

        private readonly string code;
        private readonly ExpressionParser expressionParser;
        private int index;
        private readonly StatementParser statementParser;
        private readonly TypeParser typeParser;

        public LexemeCollection Lexemes { get; }

        public ProgramNode? ParseWholeProgram()
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
            return new ProgramNode(topLevelStatements, lineNumber: 1, index: 0);
        }

        private bool TryParseNextNode([NotNullWhen(true)] out Node? node)
        {
            if (Lexemes[index] is EndOfFileLexeme)
            {
                node = null;
                return false;
            }

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
