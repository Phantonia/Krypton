using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Grammatical
{
    public sealed class ScriptParser
    {
        public ScriptParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;

            scriptNode = new ScriptNode();
        }

        private int index;
        private readonly ScriptNode scriptNode;

        public LexemeCollection Lexemes { get; }

        public Node? ParseNextNode()
        {
            return Lexemes[index] switch
            {
                OutKeywordLexeme => ParseOutStatement(),
                _ => null
            };
        }

        public SyntaxTree ParseWholeScript()
        {
            Node? nextNode = ParseNextNode();

            while (nextNode != null)
            {
                switch (nextNode)
                {
                    case StatementNode sn:
                        scriptNode.Statements.Add(sn);
                        break;
                    default: Debug.Assert(false); break;
                }

                nextNode = ParseNextNode();
            }

            return new SyntaxTree(scriptNode);
        }

        private OutStatementNode ParseOutStatement()
        {
            int lineNumberOutKeyword = Lexemes[index].LineNumber;

            index++;

            // This is temporary!!!
            Debug.Assert(Lexemes[index] is StringLiteralLexeme);

            StringLiteralLexeme sll = (Lexemes[index] as StringLiteralLexeme)!;

            index++;

            Debug.Assert(Lexemes[index] is SemicolonLexeme);

            index++;

            return new OutStatementNode(lineNumberOutKeyword, new StringLiteralExpressionNode(sll.Content, sll.LineNumber));
        }
    }
}
