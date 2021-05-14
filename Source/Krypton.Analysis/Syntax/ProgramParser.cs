//using Krypton.CompilationData;
//using Krypton.CompilationData.Syntax;
//using Krypton.CompilationData.Syntax.Clauses;
//using Krypton.CompilationData.Syntax.Declarations;
//using Krypton.CompilationData.Syntax.Expressions;
//using Krypton.CompilationData.Syntax.Statements;
//using Krypton.CompilationData.Syntax.Tokens;
//using Krypton.CompilationData.Syntax.Types;
//using Krypton.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;

//namespace Krypton.Analysis.Syntax
//{
//    internal sealed class ProgramParser
//    {
//        public ProgramParser(FinalList<Token> tokens, Analyser analyser)
//        {
//            this.analyser = analyser;
//            expressionParser = new ExpressionParser(tokens, analyser);
//            typeParser = new TypeParser(tokens, analyser);
//            statementParser = new StatementParser(tokens, expressionParser, typeParser, analyser);
//            this.tokens = tokens;
//        }

//        private readonly Analyser analyser;
//        private readonly ExpressionParser expressionParser;
//        private int index;
//        private readonly StatementParser statementParser;
//        private readonly FinalList<Token> tokens;
//        private readonly TypeParser typeParser;

//        public ProgramNode? ParseWholeProgram()
//        {
//            List<TopLevelNode> topLevelNodes = new();

//            while (TryParseNextNode(out SyntaxNode? node, out bool error))
//            {
//                if (error)
//                {
//                    return null;
//                }

//                switch (node)
//                {
//                    case StatementNode statement:
//                        topLevelNodes.Add(new TopLevelStatementNode(statement));
//                        break;
//                    case DeclarationNode declaration:
//                        topLevelNodes.Add(new TopLevelDeclarationNode(declaration));
//                        break;
//                    default:
//                        Debug.Fail("Should not have happened");
//                        return null;
//                }
//            }

//            return new ProgramNode(topLevelNodes);
//        }

//        internal ConstantDeclarationNode? ParseConstantDeclaration()
//        {
//            var constKeyword = (ReservedKeywordToken)tokens[index];

//            index++;

//            if (tokens[index] is not IdentifierToken identifier)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier,
//                //                          code,
//                //                          Lexemes[index]);
//                //return null;
//            }

//            index++;

//            AsClauseNode? asClause = null;

//            if (tokens[index] is ReservedKeywordToken { Keyword: ReservedKeyword.As } asKeyword)
//            {
//                index++;

//                TypeNode? type = typeParser.ParseNextType(ref index);

//                if (type == null)
//                {
//                    return null;
//                }

//                asClause = new AsClauseNode(asKeyword, type);
//            }

//            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Equals } equals)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.LetVariableAndConstMustBeInitialized,
//                //                          code,
//                //                          Lexemes[index]);
//                //return null;
//            }

//            index++;

//            ExpressionNode? value = expressionParser.ParseNextExpression(ref index);

//            if (value == null)
//            {
//                return null;
//            }

//            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Semicolon } semicolon)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.ExpectedSemicolon,
//                //                          code,
//                //                          Lexemes[index]);
//                //return null;
//            }

//            index++;

//            return new ConstantDeclarationNode(constKeyword, identifier, asClause, equals, value, semicolon);
//        }

//        internal FunctionDeclarationNode? ParseFunctionDeclaration()
//        {
//            var funcKeyword = (ReservedKeywordToken)tokens[index];

//            index++;

//            if (tokens[index] is not IdentifierToken functionIdentifier)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, Lexemes[index]);
//                //return null;
//            }

//            index++;

//            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening } openingParenthesis)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.ExpectedOpenParenthesis, code, Lexemes[index]);
//                //return null;
//            }

//            index++;

//            List<ParameterDeclarationNode>? parameters = null;
//            List<SyntaxCharacterToken>? commas = null;

//            SyntaxCharacterToken? closingParenthesis;

//            if (tokens[index] is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing } closingParenthesisTmp)
//            {
//                (parameters, commas, closingParenthesis) = ParseParameterList();

//                if (parameters == null)
//                {
//                    return null;
//                }

//                Debug.Assert(commas != null);
//                Debug.Assert(closingParenthesis != null);
//            }
//            else
//            {
//                index++;
//                closingParenthesis = closingParenthesisTmp;
//            }

//            AsClauseNode? returnTypeAsClause = null;

//            if (tokens[index] is ReservedKeywordToken { Keyword: ReservedKeyword.As } asKeyword)
//            {
//                index++;

//                TypeNode? returnType = typeParser.ParseNextType(ref index);

//                if (returnType == null)
//                {
//                    return null;
//                }

//                returnTypeAsClause = new AsClauseNode(asKeyword, returnType);
//            }

//            BodyNode? body = statementParser.ParseStatementBlock(ref index);

//            if (body == null)
//            {
//                return null;
//            }

//            return new FunctionDeclarationNode(funcKeyword,
//                                               functionIdentifier,
//                                               openingParenthesis,
//                                               parameters,
//                                               commas,
//                                               closingParenthesis,
//                                               returnTypeAsClause,
//                                               body);
//        }

//        private (List<ParameterDeclarationNode>? parameters,
//                 List<SyntaxCharacterToken>? commas,
//                 SyntaxCharacterToken? closingParenthesis) ParseParameterList()
//        {
//            List<ParameterDeclarationNode> parameters = new();
//            List<SyntaxCharacterToken> commas = new();

//            while (true)
//            {
//                if (tokens[index] is not IdentifierToken parameterIdentifier)
//                {
//                    throw new NotImplementedException();
//                    //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, Lexemes[index]);
//                    //return null;
//                }

//                index++;

//                if (tokens[index] is not ReservedKeywordToken { Keyword: ReservedKeyword.As } asKeyword)
//                {
//                    throw new NotImplementedException();
//                    //ErrorProvider.ReportError(ErrorCode.ExpectedKeywordAs, code, Lexemes[index]);
//                    //return null;
//                }

//                index++;

//                TypeNode? parameterType = typeParser.ParseNextType(ref index);

//                if (parameterType == null)
//                {
//                    return (null, null, null);
//                }

//                AsClauseNode asClause = new(asKeyword, parameterType);

//                ParameterDeclarationNode parameter = new(parameterIdentifier,
//                                                         asClause);

//                parameters.Add(parameter);

//                switch (tokens[index])
//                {
//                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Comma } comma:
//                        commas.Add(comma);
//                        index++;
//                        break;
//                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing } closingParenthesis:
//                        index++;
//                        return (parameters, commas, closingParenthesis);
//                    default:
//                        throw new NotImplementedException();
//                        //ErrorProvider.ReportError(ErrorCode.ExpectedCommaOrClosingParenthesis, code, Lexemes[index]);
//                        //return null;
//                }
//            }
//        }

//        private bool TryParseNextNode(out SyntaxNode? node, out bool error)
//        {
//            switch (tokens[index])
//            {
//                case EndOfFileToken:
//                    node = null;
//                    error = false;
//                    return false;
//                case ReservedKeywordToken { Keyword: ReservedKeyword.Func }:
//                    {
//                        FunctionDeclarationNode? function = ParseFunctionDeclaration();

//                        if (function == null)
//                        {
//                            error = true;
//                            node = null;
//                            return true;
//                        }

//                        node = function;
//                        error = false;
//                        return true;
//                    }
//                case ReservedKeywordToken { Keyword: ReservedKeyword.Const }:
//                    {
//                        ConstantDeclarationNode? constant = ParseConstantDeclaration();

//                        if (constant == null)
//                        {
//                            error = true;
//                            node = null;
//                            return true;
//                        }

//                        node = constant;
//                        error = false;
//                        return true;
//                    }
//            }

//            StatementNode? statementNode = statementParser.ParseNextStatement(ref index);

//            if (statementNode == null)
//            {
//                node = null;
//                error = true;
//                return true;
//            }

//            node = statementNode;
//            error = false;
//            return true;
//        }
//    }
//}
