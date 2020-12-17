using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System.Diagnostics;

namespace Krypton.Analysis.Grammatical
{
    public sealed class ScriptParser
    {
        public ScriptParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;

            expressionParser = new ExpressionParser(lexemes);
            scriptNode = new ScriptNode();
            typeParser = new TypeParser(lexemes);
        }

        private readonly ExpressionParser expressionParser;
        private int index;
        private readonly ScriptNode scriptNode;
        private readonly TypeParser typeParser;

        public LexemeCollection Lexemes { get; }

        public Node? ParseNextNode()
        {
            return Lexemes[index] switch
            {
                OutKeywordLexeme => ParseOutStatement(),
                VarKeywordLexeme => ParseVariableDeclarationStatement(),
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

        private VariableDeclarationStatementNode? ParseVariableDeclarationStatement()
        {
            int lineNumber = Lexemes[index].LineNumber;

            index++;
            Lexeme? current = Lexemes.TryGet(index);

            if (current is IdentifierLexeme idl)
            {
                index++;
                current = Lexemes.TryGet(index);

                string variableName = idl.Content;

                if (current is EqualsLexeme)
                {
                    return HandleAssignedValue(type: null);
                }
                else if (current is AsKeywordLexeme)
                {
                    index++;

                    TypeNode? type = typeParser.ParseNextType(ref index);

                    if (type == null)
                    {
                        return null;
                    }

                    current = Lexemes.TryGet(index);

                    if (current is EqualsLexeme)
                    {
                        return HandleAssignedValue(type);
                    }
                    else if (current is SemicolonLexeme)
                    {
                        return new VariableDeclarationStatementNode(variableName, type, value: null, lineNumber);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                VariableDeclarationStatementNode? HandleAssignedValue(TypeNode? type)
                {
                    index++;

                    ExpressionNode? assignedValue = expressionParser.ParseNextExpression(ref index);

                    if (assignedValue == null)
                    {
                        return null;
                    }

                    current = Lexemes.TryGet(index);

                    if (current is SemicolonLexeme)
                    {
                        return new VariableDeclarationStatementNode(variableName, type, assignedValue, lineNumber);
                    }
                    else
                    {
                        ErrorProvider.ReportMissingSemicolonError(current?.Content ?? "", lineNumber);
                        return null;
                    }
                }
            }
            else
            {
                ErrorProvider.ReportMissingIdentifier(current?.Content ?? "", current?.LineNumber ?? Lexemes[index - 1].LineNumber);
                return null;
            }
        }
    }
}
