using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Framework;
using Krypton.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Syntactical
{
    public sealed class ExpressionParser
    {
        public ExpressionParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;
        }

        public LexemeCollection Lexemes { get; }

        public ExpressionNode? ParseNextExpression(ref int index)
        {
            Debug.Assert(Lexemes.Count > index & index >= 0);
            ExpressionNode? expression = ParseNextExpressionInternal(ref index);
            index++;
            return expression;
        }

        private ExpressionNode? ParseNextExpressionInternal(ref int index)
        {
            ExpressionNode? expression = ParseSubExpression(ref index);

            if (expression == null)
            {
                return null;
            }

            ParseAfterSubExpression(ref expression, ref index);

            if (expression is BinaryOperationChain chain)
            {
                expression = chain.Resolve();
            }

            return expression;
        }

        private void ParseAfterSubExpression(ref ExpressionNode? expression, ref int index, bool includeOperations = true)
        {
            while (true)
            {
                switch (Lexemes[index + 1])
                {
                    case IOperatorLexeme operatorLexeme when includeOperations:
                        index++;
                        ParseOperationChain(operatorLexeme, ref expression, ref index);
                        return;
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening }:
                        index++;
                        ParseFunctionCall(ref expression, ref index);
                        continue;
                    default:
                        return;
                }
            }
        }

        private void ParseFunctionCall(ref ExpressionNode? expression, ref int index)
        {
            if (expression == null)
            {
                return;
            }

            int lineNumber = Lexemes[index].LineNumber;
            int nodeIndex = Lexemes[index].Index;

            index++;

            if (Lexemes.TryGet(index) is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
            {
                expression = new FunctionCallExpressionNode(expression, lineNumber, nodeIndex);
                return;
            }

            List<ExpressionNode> arguments = new();

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

                switch (Lexemes.TryGet(index))
                {
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Comma }:
                        break;
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing }:
                        expression = new FunctionCallExpressionNode(expression, arguments, lineNumber, nodeIndex);
                        return;
                    case { }:
                        throw new NotImplementedException("Error: missing comma or parenthesis");
                    case null:
                        throw new NotImplementedException("Error: missing comma or parenthesis");
                }

                index++;
            }
        }

        private void ParseOperationChain(IOperatorLexeme operatorLexeme, ref ExpressionNode? expression, ref int index)
        {
            if (expression == null)
            {
                return;
            }

            if (expression is not BinaryOperationChain chain)
            {
                chain = new(operatorLexeme.LineNumber, operatorLexeme.Index);
                chain.AddOperand(expression);
            }

            chain.AddOperator(operatorLexeme);

            index++;

            ExpressionNode? nextOperand = ParseSubExpression(ref index);

            if (nextOperand == null)
            {
                return;
            }

            chain.AddOperand(nextOperand);

            expression = chain;

            if (Lexemes[index + 1] is IOperatorLexeme opl)
            {
                index++;
                ParseOperationChain(opl, ref expression, ref index);
            }
        }

        private ExpressionNode? ParseSubExpression(ref int index)
        {
            switch (Lexemes[index])
            {
                case BooleanLiteralLexeme booleanLiteral:
                    return new BooleanLiteralExpressionNode(booleanLiteral.Value, booleanLiteral.LineNumber, booleanLiteral.Index);
                case IntegerLiteralLexeme integerLiteral:
                    return new IntegerLiteralExpressionNode(integerLiteral.Value, integerLiteral.LineNumber, integerLiteral.Index);
                case StringLiteralLexeme stringLiteral:
                    return new StringLiteralExpressionNode(stringLiteral.Value, stringLiteral.LineNumber, stringLiteral.Index);
                case CharLiteralLexeme charLiteral: 
                    return new CharLiteralExpressionNode(charLiteral.Value, charLiteral.LineNumber, charLiteral.Index);
                case ImaginaryLiteralLexeme imaginaryLiteral:
                    return new ImaginaryLiteralExpressionNode(imaginaryLiteral.Value, imaginaryLiteral.LineNumber, imaginaryLiteral.Index);
                case RationalLiteralLexeme rationalLiteral:
                    return new RationalLiteralExpressionNode(rationalLiteral.Value, rationalLiteral.LineNumber, rationalLiteral.Index);
                case IdentifierLexeme identifierLexeme:
                    return new IdentifierExpressionNode(identifierLexeme.Content, identifierLexeme.LineNumber, identifierLexeme.Index);
                case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening }:
                    {
                        index++;

                        ExpressionNode? expression = ParseNextExpressionInternal(ref index);

                        index++;

                        Lexeme? nextLexeme = Lexemes.TryGet(index);

                        if (nextLexeme is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
                        {
                            throw new NotImplementedException("Error: missing closing parenthesis");
                        }

                        return expression;
                    }
                case CharacterOperatorLexeme { Operator: Operator.Tilde }:
                    {
                        index++;

                        ExpressionNode? operand = ParseNextExpressionInternal(ref index);

                        if (operand != null)
                        {
                            return new UnaryOperationExpressionNode(operand, Operator.Tilde, Lexemes[index].LineNumber, Lexemes[index].LineNumber);
                        }

                        return null;
                    }
                case CharacterOperatorLexeme { Operator: Operator.Minus }:
                    {
                        index++;

                        ExpressionNode? operand = ParseSubExpression(ref index);
                        ParseAfterSubExpression(ref operand, ref index, includeOperations: false);

                        if (operand != null)
                        {
                            return new UnaryOperationExpressionNode(operand, Operator.Minus, Lexemes[index].LineNumber, Lexemes[index].Index);
                        }

                        return null;
                    }
                default:
                    throw new NotImplementedException("Error: unexpected expression term");
            }
        }
    }
}
