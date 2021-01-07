using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Grammatical
{
    public sealed class ScriptParser
    {
        public ScriptParser(LexemeCollection lexemes)
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

        public SyntaxTree? ParseWholeScript()
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

            BlockStatementNode topLevelStatements = new(statements, statements.TryGet(0)?.LineNumber ?? 0);
            ScriptNode scriptNode = new(topLevelStatements, topLevelStatements.LineNumber);
            return new SyntaxTree(scriptNode);
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
