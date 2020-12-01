using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
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

        public SyntaxTree? ParseWholeScript()
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

            if (Lexemes[index] is EndOfFileLexeme)
            {
                return new SyntaxTree(scriptNode);
            }
            else
            {
                return null;
            }
        }

        private OutStatementNode? ParseOutStatement()
        {
            int lineNumberOutKeyword = Lexemes[index].LineNumber;

            index++;

            // This is temporary!!!
            Debug.Assert(Lexemes[index] is StringLiteralLexeme);

            StringLiteralLexeme sll = (Lexemes[index] as StringLiteralLexeme)!;

            index++;

            if (Lexemes[index] is not SemicolonLexeme)
            {
                ErrorProvider.ReportMissingSemicolonError(Lexemes[index].Content, Lexemes[index].LineNumber);

                if (Lexemes[index] is EndOfFileLexeme)
                {
                    index--;
                }

                return null;
            }

            index++;
            return new OutStatementNode(lineNumberOutKeyword, new StringLiteralExpressionNode(sll.Content, sll.LineNumber));
        }
    }
}
