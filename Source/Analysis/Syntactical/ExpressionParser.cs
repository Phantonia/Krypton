using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ChainNode = Krypton.CompilationData
                         .Syntax
                         .Expressions
                         .UtilityWrapperExpressionNode<Krypton.Analysis
                                                              .Syntactical
                                                              .BinaryOperationChain>;

namespace Krypton.Analysis.Syntactical
{
    internal sealed class ExpressionParser
    {
        public ExpressionParser(FinalList<Token> tokens, Analyser analyser)
        {
            this.tokens = tokens;
            this.analyser = analyser;
        }

        private readonly Analyser analyser;
        private readonly FinalList<Token> tokens;

        public ExpressionNode? ParseNextExpression(ref int index)
        {
            Debug.Assert(tokens.Count > index & index >= 0);
            ExpressionNode? expression = ParseNextExpressionInternal(ref index);
            index++;
            return expression;
        }

        private ExpressionNode? ParseNextExpressionInternal(ref int index,
                                                            bool includeOperations = true)
        {
            ExpressionNode? expression = ParseSubExpression(ref index);

            if (expression == null)
            {
                return null;
            }

            ParseAfterSubExpression(ref expression, ref index, includeOperations);

            if (expression is ChainNode chain)
            {
                expression = chain.Value.Resolve();
            }

            return expression;
        }

        private void ParseAfterSubExpression(ref ExpressionNode? expression,
                                             ref int index,
                                             bool includeOperations = true)
        {
            while (true)
            {
                switch (tokens.TryGet(index + 1))
                {
                    case OperatorToken operatorToken when includeOperations:
                        index++;
                        ParseOperationChain(operatorToken, ref expression, ref index);
                        return;
                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening } openingParenthesis:
                        index++;
                        ParseFunctionCall(ref expression, ref index, openingParenthesis);
                        continue;
                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Dot } dot:
                        index++;
                        ParsePropertyGetAccess(ref expression, ref index, dot);
                        continue;
                    default:
                        return;
                }
            }
        }

        private void ParseFunctionCall(ref ExpressionNode? expression, ref int index, SyntaxCharacterToken openingParenthesis)
        {
            Debug.Assert(openingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisOpening);

            if (expression == null)
            {
                return;
            }

            index++;

            {
                if (tokens.TryGet(index) is SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing } closingParenthesis)
                {
                    expression = new InvocationExpressionNode(expression,
                                                              openingParenthesis,
                                                              arguments: null,
                                                              commas: null,
                                                              closingParenthesis);
                    return;
                }
            }

            List<ExpressionNode> arguments = new();
            List<SyntaxCharacterToken> commas = new();

            while (true)
            {
                ExpressionNode? nextExpression = ParseNextExpressionInternal(ref index);

                if (nextExpression == null)
                {
                    expression = null;
                    return;
                }

                arguments.Add(nextExpression);

                index++;

                switch (tokens.TryGet(index))
                {
                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.Comma } comma:
                        commas.Add(comma);
                        break;
                    case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing } closingParenthesis:
                        expression = new InvocationExpressionNode(expression, openingParenthesis, arguments, commas, closingParenthesis);
                        return;
                    case Token token:
                        throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.ExpectedCommaOrClosingParenthesis, code, lexeme);
                    //expression = null;
                    //return;
                    case null:
                        Debug.Fail(message: null);
                        return;
                }

                index++;
            }
        }

        private void ParseOperationChain(OperatorToken operatorToken, ref ExpressionNode? expression, ref int index)
        {
            if (expression == null)
            {
                return;
            }

            if (expression is not ChainNode chain)
            {
                BinaryOperationChain operationChain = new();
                chain = new(operationChain);
                chain.Value.AddOperand(expression);
            }

            chain.Value.AddOperator(operatorToken);

            index++;

            ExpressionNode? nextOperand = ParseNextExpressionInternal(ref index, includeOperations: false);

            if (nextOperand == null)
            {
                expression = null;
                return;
            }

            chain.Value.AddOperand(nextOperand);

            expression = chain;

            if (tokens[index + 1] is OperatorToken nextOperatorToken)
            {
                index++;
                ParseOperationChain(nextOperatorToken, ref expression, ref index);
            }
        }

        private void ParsePropertyGetAccess(ref ExpressionNode? expression, ref int index, SyntaxCharacterToken dot)
        {
            if (expression == null)
            {
                return;
            }

            index++;

            if (tokens[index] is not IdentifierToken identifierToken)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier, code, tokens[index]);
                //expression = null;
                //return;
            }

            expression = new PropertyGetAccessExpressionNode(expression, dot, identifierToken);
        }

        private ExpressionNode? ParseSubExpression(ref int index)
        {
            switch (tokens[index])
            {
                case LiteralToken<bool> booleanLiteral:
                    return new LiteralExpressionNode<bool>(booleanLiteral);
                case LiteralToken<long> integerLiteral:
                    return new LiteralExpressionNode<long>(integerLiteral);
                case LiteralToken<string> stringLiteral:
                    return new LiteralExpressionNode<string>(stringLiteral);
                case LiteralToken<char> charLiteral:
                    return new LiteralExpressionNode<char>(charLiteral);
                case LiteralToken<RationalComplex> imaginaryLiteral:
                    return new LiteralExpressionNode<RationalComplex>(imaginaryLiteral);
                case LiteralToken<Rational> rationalLiteral:
                    return new LiteralExpressionNode<Rational>(rationalLiteral);
                case IdentifierToken identifierToken:
                    return new IdentifierExpressionNode(identifierToken);
                case SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening }:
                    {
                        index++;

                        ExpressionNode? expression = ParseNextExpressionInternal(ref index);

                        if (expression == null)
                        {
                            return null;
                        }

                        index++;

                        Token? nextLexeme = tokens.TryGet(index);

                        if (nextLexeme is not SyntaxCharacterToken { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
                        {
                            if (nextLexeme == null)
                            {
                                nextLexeme = tokens[^1];
                            }
                            throw new NotImplementedException();
                            //ErrorProvider.ReportError(ErrorCode.ExpectedClosingParenthesis, code, nextLexeme);
                            //return null;
                        }

                        return expression;
                    }
                case OperatorToken { Operator: Operator.Tilde } tildeOperator:
                    return ParseUnaryOperation(ref index, tildeOperator);
                case OperatorToken { Operator: Operator.Minus } minusOperator:
                    return ParseUnaryOperation(ref index, minusOperator);
                case OperatorToken { Operator: Operator.NotKeyword } notOperator:
                    return ParseUnaryOperation(ref index, notOperator);
                case EndOfFileToken endOfFileLexeme:
                    throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ExpectedExpressionTerm, code, endOfFileLexeme);
                //return null;
                case var lexeme:
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.UnexpectedExpressionTerm, code, lexeme);
                    //return null;
            }
        }

        private UnaryOperationExpressionNode? ParseUnaryOperation(ref int index, OperatorToken @operator)
        {
            index++;

            ExpressionNode? operand = ParseSubExpression(ref index);

            if (operand == null)
            {
                return null;
            }

            ParseAfterSubExpression(ref operand, ref index, includeOperations: false);

            if (operand == null)
            {
                return null;
            }

            return new UnaryOperationExpressionNode(@operator, operand);
        }
    }
}
