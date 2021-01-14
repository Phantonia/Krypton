using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.UnaryOperations;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Grammatical
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
            ExpressionNode? root = ParseNextExpressionInternal(ref index);
            index++;
            return root;
        }

        private ExpressionNode? ParseNextExpressionInternal(ref int index)
        {
            ExpressionNode? root = ParseSubExpression(ref index);

            if (root == null)
            {
                return null;
            }

            ParseAfterSubExpression(ref root, ref index);

            if (root is BinaryOperationChainExpressionNode chain)
            {
                root = chain.Resolve();
            }

            return root;
        }

        private void ParseAfterSubExpression(ref ExpressionNode? root, ref int index, bool includeOperations = true)
        {
            while (true)
            {
                switch (Lexemes[index + 1])
                {
                    case IOperatorLexeme opl when includeOperations:
                        index++;
                        ParseOperationChain(opl, ref root, ref index);
                        return;
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening }:
                        index++;
                        ParseFunctionCall(ref root, ref index);
                        continue;
                    default:
                        return;
                }
            }
        }

        private void ParseFunctionCall(ref ExpressionNode? root, ref int index)
        {
            if (root == null)
            {
                return;
            }

            int lineNumber = Lexemes[index].LineNumber;

            index++;

            if (Lexemes.TryGet(index) is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
            {
                root = new FunctionCallExpressionNode(root, lineNumber);
                return;
            }

            List<ExpressionNode> arguments = new();

            while (true)
            {
                ExpressionNode? nextExpression = ParseNextExpressionInternal(ref index);

                if (nextExpression == null)
                {
                    root = null;
                    return;
                }

                arguments.Add(nextExpression);

                index++;

                switch (Lexemes.TryGet(index))
                {
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Comma }:
                        break;
                    case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing }:
                        root = new FunctionCallExpressionNode(root, arguments, lineNumber);
                        return;
                    case { }:
                        ErrorProvider.ReportMissingCommaOrParenthesis(Lexemes[index].Content, Lexemes[index].LineNumber);
                        break;
                    case null:
                        ErrorProvider.ReportMissingCommaOrParenthesis("", Lexemes[index - 1].LineNumber);
                        break;
                }

                index++;
            }
        }

        private void ParseOperationChain(IOperatorLexeme opLexeme, ref ExpressionNode? root, ref int index)
        {
            if (root == null)
            {
                return;
            }

            if (root is not BinaryOperationChainExpressionNode chain)
            {
                chain = new(opLexeme.LineNumber);
                chain.AddOperand(root);
            }

            chain.AddOperator(opLexeme);

            index++;

            ExpressionNode? nextOperand = ParseSubExpression(ref index);
                            
            if (nextOperand == null)
            {
                return;
            }

            chain.AddOperand(nextOperand);

            root = chain;

            if (Lexemes[index + 1] is IOperatorLexeme opl)
            {
                index++;
                ParseOperationChain(opl, ref root, ref index);
            }
        }

        private ExpressionNode? ParseSubExpression(ref int index)
        {
            switch (Lexemes[index])
            {
                case BooleanLiteralLexeme bll:
                    return new BooleanLiteralExpressionNode(bll.Value, bll.LineNumber);
                case IntegerLiteralLexeme ill:
                    return new IntegerLiteralExpressionNode(ill.Value, ill.LineNumber);
                case StringLiteralLexeme sll:
                    return new StringLiteralExpressionNode(sll.Value, sll.LineNumber);
                case CharLiteralLexeme cll:
                    return new CharLiteralExpressionNode(cll.Value, cll.LineNumber);
                case ImaginaryLiteralLexeme imll:
                    return new ImaginaryLiteralExpressionNode(imll.Value, imll.LineNumber);
                case RationalLiteralLexeme rll:
                    return new RationalLiteralExpressionNode(rll.Value, rll.LineNumber);
                case IdentifierLexeme idll:
                    return new IdentifierExpressionNode(idll.Content, idll.LineNumber);
                case SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening }:
                    {
                        index++;
                        ExpressionNode? root = ParseNextExpressionInternal(ref index);

                        index++;

                        Lexeme? nextLexeme = Lexemes.TryGet(index);

                        if (nextLexeme is not SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing })
                        {
                            ErrorProvider.ReportMissingClosingParenthesis(nextLexeme?.Content ?? "", nextLexeme?.LineNumber ?? Lexemes[index - 1].LineNumber);
                            return null;
                        }
                        return root;
                    }
                case CharacterOperatorLexeme { Operator: CharacterOperator.Tilde }:
                    {
                        index++;
                        ExpressionNode? operand = ParseNextExpressionInternal(ref index);

                        if (operand != null)
                        {
                            return new BitwiseNotUnaryOperationExpressionNode(operand, Lexemes[index].LineNumber);
                        }

                        return null;
                    }
                case CharacterOperatorLexeme { Operator: CharacterOperator.Minus }:
                    {
                        index++;
                        ExpressionNode? operand = ParseSubExpression(ref index);
                        ParseAfterSubExpression(ref operand, ref index, includeOperations: false);

                        if (operand != null)
                        {
                            return new NegationUnaryOperationExpressionNode(operand, Lexemes[index].LineNumber);
                        }

                        return null;
                    }
                default:
                    ErrorProvider.ReportUnexpectedExpressionTerm(Lexemes[index]);
                    return null;
            }
        }
    }
}
