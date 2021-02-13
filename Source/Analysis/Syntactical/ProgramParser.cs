using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            statementParser = new StatementParser(lexemes, expressionParser, typeParser, code);
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
            List<FunctionDeclarationNode> functions = new();

            while (TryParseNextNode(out Node? node, out bool error))
            {
                if (error)
                {
                    return null;
                }

                switch (node)
                {
                    case StatementNode statement:
                        statements.Add(statement);
                        break;
                    case FunctionDeclarationNode function:
                        functions.Add(function);
                        break;
                    default:
                        Debug.Fail("Should not have happened");
                        return null;
                }
            }

            StatementCollectionNode topLevelStatements = new(statements);
            return new ProgramNode(topLevelStatements, functions, lineNumber: 1, index: 0);
        }

        internal FunctionDeclarationNode? ParseFunctionDeclaration()
        {
            int lineNumber = Lexemes[index].LineNumber;
            int nodeIndex = Lexemes[index].Index;

            index++;

            if (Lexemes[index] is not IdentifierLexeme functionIdentifier)
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, Lexemes[index]);
                return null;
            }

            UnboundIdentifierNode functionIdentifierNode = new(functionIdentifier.Content,
                                                               functionIdentifier.LineNumber,
                                                               functionIdentifier.Index);

            index++;

            if (Lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedOpenParenthesis, code, Lexemes[index]);
                return null;
            }

            index++;

            List<ParameterDeclarationNode>? parameters = null;

            if (Lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
            {
                parameters = ParseParameterList();

                if (parameters == null)
                {
                    return null;
                }
            }
            else
            {
                index++;
            }

            TypeSpecNode? returnType = null;

            if (Lexemes[index] is KeywordLexeme { Keyword: ReservedKeyword.As })
            {
                index++;

                returnType = typeParser.ParseNextType(ref index);

                if (returnType == null)
                {
                    return null;
                }
            }

            StatementCollectionNode? body = statementParser.ParseStatementBlock(ref index);

            if (body == null)
            {
                return null;
            }

            return new FunctionDeclarationNode(functionIdentifierNode, parameters, returnType, body, lineNumber, nodeIndex);
        }

        private List<ParameterDeclarationNode>? ParseParameterList()
        {
            List<ParameterDeclarationNode> parameters = new();

            while (true)
            {
                if (Lexemes[index] is not IdentifierLexeme parameterIdentifier)
                {
                    ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, Lexemes[index]);
                    return null;
                }

                index++;

                if (Lexemes[index] is not KeywordLexeme { Keyword: ReservedKeyword.As })
                {
                    ErrorProvider.ReportError(ErrorCode.ExpectedKeywordAs, code, Lexemes[index]);
                    return null;
                }

                index++;

                TypeSpecNode? parameterType = typeParser.ParseNextType(ref index);

                if (parameterType == null)
                {
                    return null;
                }

                UnboundIdentifierNode parameterIdentifierNode = new(parameterIdentifier.Content,
                                                                    parameterIdentifier.LineNumber,
                                                                    parameterIdentifier.Index);
                ParameterDeclarationNode parameter = new(parameterIdentifierNode,
                                                         parameterType,
                                                         parameterIdentifier.LineNumber,
                                                         parameterIdentifier.Index);

                parameters.Add(parameter);

                switch (Lexemes[index])
                {
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Comma }:
                        index++;
                        break;
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing }:
                        index++;
                        return parameters;
                    default:
                        ErrorProvider.ReportError(ErrorCode.ExpectedCommaOrClosingParenthesis, code, Lexemes[index]);
                        return null;
                }
            }
        }

        private bool TryParseNextNode(out Node? node, out bool error)
        {
            switch (Lexemes[index])
            {
                case EndOfFileLexeme:
                    node = null;
                    error = false;
                    return false;
                case KeywordLexeme { Keyword: ReservedKeyword.Func }:
                    break;
            }

            StatementNode? statementNode = statementParser.ParseNextStatement(ref index);

            if (statementNode == null)
            {
                node = null;
                error = true;
                return true;
            }

            node = statementNode;
            error = false;
            return true;
        }
    }
}
