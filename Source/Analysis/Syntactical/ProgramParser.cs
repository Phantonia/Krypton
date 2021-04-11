using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Declarations;
using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Types;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Syntactical
{
    internal sealed class ProgramParser
    {
        public ProgramParser(LexemeCollection lexemes, string code)
        {
            Lexemes = lexemes;

            this.code = code;
            expressionParser = new ExpressionParser(lexemes, code);
            typeParser = new TypeParser(lexemes, code);
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
            List<TopLevelNode> topLevelNodes = new();

            while (TryParseNextNode(out SyntaxNode? node, out bool error))
            {
                if (error)
                {
                    return null;
                }

                switch (node)
                {
                    case StatementNode statement:
                        topLevelNodes.Add(new TopLevelStatementNode(statement));
                        break;
                    case DeclarationNode declaration:
                        topLevelNodes.Add(new TopLevelDeclarationNode(declaration));
                        break;
                    default:
                        Debug.Fail("Should not have happened");
                        return null;
                }
            }

            ProgramNode program = new(topLevelNodes);

            return program;
        }

        internal ConstantDeclarationNode? ParseConstantDeclaration()
        {
            int lineNumber = Lexemes[index].LineNumber;
            int nodeIndex = Lexemes[index].Index;

            index++;

            if (Lexemes[index] is not IdentifierLexeme identifier)
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier,
                                          code,
                                          Lexemes[index]);
                return null;
            }

            index++;

            TypeNode? type = null;

            if (Lexemes[index] is KeywordLexeme { Keyword: ReservedKeyword.As })
            {
                index++;

                type = typeParser.ParseNextType(ref index);

                if (type == null)
                {
                    return null;
                }
            }

            if (Lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals })
            {
                ErrorProvider.ReportError(ErrorCode.LetVariableAndConstMustBeInitialized,
                                          code,
                                          Lexemes[index]);
                return null;
            }

            index++;

            ExpressionNode? value = expressionParser.ParseNextExpression(ref index);

            if (value == null)
            {
                return null;
            }

            if (Lexemes[index] is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Semicolon })
            {
                ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon,
                                          code,
                                          Lexemes[index]);
                return null;
            }

            index++;

            UnboundIdentifierNode identifierNode = new(identifier.Content, identifier.LineNumber, identifier.Index);
            return new ConstantDeclarationNode(identifierNode, type, value, lineNumber, nodeIndex);
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

        private bool TryParseNextNode(out SyntaxNode? node, out bool error)
        {
            switch (Lexemes[index])
            {
                case EndOfFileLexeme:
                    node = null;
                    error = false;
                    return false;
                case KeywordLexeme { Keyword: ReservedKeyword.Func }:
                    {
                        FunctionDeclarationNode? function = ParseFunctionDeclaration();

                        if (function == null)
                        {
                            error = true;
                            node = null;
                            return true;
                        }

                        node = function;
                        error = false;
                        return true;
                    }
                case KeywordLexeme { Keyword: ReservedKeyword.Const }:
                    {
                        ConstantDeclarationNode? constant = ParseConstantDeclaration();

                        if (constant == null)
                        {
                            error = true;
                            node = null;
                            return true;
                        }

                        node = constant;
                        error = false;
                        return true;
                    }
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
