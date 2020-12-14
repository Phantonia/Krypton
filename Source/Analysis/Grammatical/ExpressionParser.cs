using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System.Collections.Generic;

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

            CheckForLexemeAfterSubExpression:
            switch (Lexemes[index + 1])
            {
                case IOperatorLexeme opl:
                    index++;
                    ParseOperationChain(opl, ref root, ref index);
                    break;
                case ParenthesisOpeningLexeme:
                    index++;
                    ParseFunctionCall(ref root, ref index);
                    goto CheckForLexemeAfterSubExpression;
            }

            if (root is BinaryOperationChainNode chain)
            {
                root = chain.Resolve();
            }

            return root;
        }

        private void ParseFunctionCall(ref ExpressionNode? root, ref int index)
        {
            if (root == null)
            {
                return;
            }

            int lineNumber = Lexemes[index].LineNumber;

            index++;

            if (Lexemes.TryGet(index) is ParenthesisClosingLexeme)
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
                    case CommaLexeme:
                        break;
                    case ParenthesisClosingLexeme:
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

            if (root is not BinaryOperationChainNode chain)
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
                case ParenthesisOpeningLexeme:
                    {
                        index++;
                        ExpressionNode? root = ParseNextExpressionInternal(ref index);

                        index++;

                        Lexeme? nextLexeme = Lexemes.TryGet(index);

                        if (nextLexeme is not ParenthesisClosingLexeme)
                        {
                            ErrorProvider.ReportMissingClosingParenthesis(nextLexeme?.Content ?? "", nextLexeme?.LineNumber ?? Lexemes[index - 1].LineNumber);
                            return null;
                        }
                        return root;
                    }
                default:
                    ErrorProvider.ReportUnexpectedExpressionTerm(Lexemes[index]);
                    return null;
            }
        }
    }
}
