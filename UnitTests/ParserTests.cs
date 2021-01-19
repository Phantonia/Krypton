﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.BinaryOperations;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Expressions.UnaryOperations;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using NUnit.Framework;

namespace UnitTests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void SimpleExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("3").LexAll();

            Assert.AreEqual(2, lexemes.Count);
            Assert.IsAssignableFrom<IntegerLiteralLexeme>(lexemes[0]);
            Assert.IsAssignableFrom<EndOfFileLexeme>(lexemes[1]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(3, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void BracketedExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("(4)").LexAll();

            Assert.AreEqual(4, lexemes.Count);
            Assert.IsTrue(lexemes[0] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening });
            Assert.IsAssignableFrom<IntegerLiteralLexeme>(lexemes[1]);
            Assert.IsTrue(lexemes[2] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing });
            Assert.IsAssignableFrom<EndOfFileLexeme>(lexemes[3]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(4, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void IllegalExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("(5").LexAll();

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNull(root);
        }

        [Test]
        public void SeveralNestedParensTest()
        {
            LexemeCollection lexemes = new Lexer("((6))").LexAll();

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(6, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void ChainResolutionTest()
        {
            BinaryOperationChainExpressionNode chain = new(1); // Arbitrary line number
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new CharacterOperatorLexeme(CharacterOperator.Plus, 1));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new CharacterOperatorLexeme(CharacterOperator.Asterisk, 1));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            // 4 + 4 * 4 = 20

            ExpressionNode node =  chain.Resolve();

            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(node);

            BinaryOperationExpressionNode boen = (node as BinaryOperationExpressionNode)!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(boen.Left);
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(boen.Right);
        }

        [Test]
        public void OperationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("3 + 4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            var aboen = (AdditionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Right);

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.Right).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithBracketsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            var aboen = (AdditionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Right);

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.Right).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithIndividualBracketsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3) + (4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            var aboen = (AdditionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Right);

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.Right).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void LongerOperationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("3 + 4 * 5").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            var aboen = (AdditionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(aboen.Right);

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            Assert.AreEqual(3, left);
        }

        [Test]
        public void LongerOperationWithBracketsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4 * 5)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            var aboen = (AdditionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(aboen.Right);

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            Assert.AreEqual(3, left);
        }

        [Test]
        public void OperationWithNestedOperationsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4) * 5").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(root);

            var mboen = (MultiplicationBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(mboen.Left);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(mboen.Right);

            long right = ((IntegerLiteralExpressionNode)mboen.Right).Value;
            Assert.AreEqual(5, right);

            var aboen = (AdditionBinaryOperationExpressionNode)mboen.Left;

            long left = ((IntegerLiteralExpressionNode)aboen.Left).Value;
            right = ((IntegerLiteralExpressionNode)aboen.Right).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithSubExpressionsOnBothSidesTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4) / (5 - 6)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<RationalDivisionBinaryOperationExpressionNode>(root);

            var mboen = (RationalDivisionBinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(mboen.Left);
            Assert.IsAssignableFrom<SubtractionBinaryOperationExpressionNode>(mboen.Right);
        }

        [Test]
        public void IntegerDivisionOperationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4) Div (5 - 6)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<IntegerDivisionBinaryOperationExpressionNode>(root);

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(mboen.Left);
            Assert.IsAssignableFrom<SubtractionBinaryOperationExpressionNode>(mboen.Right);
        }

        [Test]
        public void BitwiseOperatorsPrecedenceTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("3 & 4 + 5").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<BitwiseAndBinaryOperationExpressionNode>(root);
        }

        [Test]
        public void RangeTest()
        {
            int index = 3;
            
            //                                       0   1 2 3 4 5 6 7 8
            ExpressionParser parser = new(new Lexer("Var x = 5 | 8 + 9;").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<BitwiseOrBinaryOperationExpressionNode>(root);

            Assert.AreEqual(index, 8);
        }

        [Test]
        public void LogicalOperatorsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("True And False Or False").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<LogicalOrBinaryOperationExpressionNode>(root);

            BinaryOperationExpressionNode orOperation = (BinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<BooleanLiteralExpressionNode>(orOperation.Right);
            Assert.IsAssignableFrom<LogicalAndBinaryOperationExpressionNode>(orOperation.Left);
        }

        [Test]
        public void StringTest()
        {
            int index = 0;

            LexemeCollection lexemes = new Lexer(@"""x""").LexAll();

            ExpressionParser parser = new(lexemes);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<StringLiteralExpressionNode>(root);
        }

        [Test]
        public void IdentifierTest()
        {
            int index = 0;

            ExpressionParser parser = new(new Lexer("x * 4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(root);

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<IdentifierExpressionNode>(op.Left);

            IdentifierExpressionNode id = (IdentifierExpressionNode)op.Left;

            Assert.AreEqual("x", id.IdentifierNode.Identifier);
        }

        [Test]
        public void MoreComplexIdentifierTest()
        {
            int index = 0;

            ExpressionParser parser = new(new Lexer("x * y & (z - 1)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<BitwiseAndBinaryOperationExpressionNode>(root);

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;
            Assert.IsAssignableFrom<MultiplicationBinaryOperationExpressionNode>(op.Left);

            BinaryOperationExpressionNode op2 = (BinaryOperationExpressionNode)op.Left;
            Assert.DoesNotThrow(() =>
            {
                Assert.AreEqual("x", ((IdentifierExpressionNode)op2.Left).IdentifierNode.Identifier);
            });

            Assert.IsAssignableFrom<SubtractionBinaryOperationExpressionNode>(op.Right);
        }

        [Test]
        public void BracketedIdentifierTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(hello)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<IdentifierExpressionNode>(root);
        }

        [Test]
        public void ErrorIdentifierTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(hello +").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.IsNull(root);
        }

        [Test]
        public void ErrorTest()
        {
            ErrorEventHandler handler = e =>
            {
                Assert.AreEqual(1, e.LineNumber);
                Assert.AreEqual(ErrorCode.ExpectedClosingParenthesis, e.ErrorCode);
            };

            ErrorProvider.Error += handler;

            int index = 0;
            ExpressionParser parser = new(new Lexer("(4;").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.IsNull(root);

            ErrorProvider.Error -= handler;
        }

        [Test]
        public void FunctionCallTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output()").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsAssignableFrom<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.IsNull(call.Arguments);
        }

        [Test]
        public void FunctionCallWithOneArgumentTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsAssignableFrom<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(1, call.Arguments!.Count);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(call.Arguments[0]);
        }

        [Test]
        public void FunctionCallWithTwoArgumentsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer(@"Output(4, ""xyz"")").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsAssignableFrom<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(2, call.Arguments!.Count);
            Assert.IsAssignableFrom<IntegerLiteralExpressionNode>(call.Arguments[0]);
            Assert.IsAssignableFrom<StringLiteralExpressionNode>(call.Arguments[1]);
        }

        [Test]
        public void FunctionCallWithErrorTest()
        {
            ErrorEventHandler handler = e =>
            {
                Assert.AreEqual(ErrorCode.UnexpectedExpressionTerm, e.ErrorCode);
                Assert.AreEqual(1, e.LineNumber);
            };

            ErrorProvider.Error += handler;

            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4, )").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.IsNull(root);

            ErrorProvider.Error -= handler;
        }

        [Test]
        public void FunctionCallWithNestedExpressionAsArgumentTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4 + 5 * 6)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsAssignableFrom<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(1, call.Arguments!.Count);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(call.Arguments[0]);
        }


        [Test]
        public void NestedFunctionCallTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4) + 8").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<AdditionBinaryOperationExpressionNode>(root);

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsAssignableFrom<FunctionCallExpressionNode>(op.Left);
        }

        [Test]
        public void ChainedFunctionCallsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4)(True)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsAssignableFrom<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsAssignableFrom<FunctionCallExpressionNode>(call.FunctionExpression);
        }

        [Test]
        public void AndTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x And y").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<LogicalAndBinaryOperationExpressionNode>(root);
        }

        [Test]
        public void UnaryNegationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<NegationUnaryOperationExpressionNode>(root);

            var neg = (NegationUnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(neg.Operand);

            var inl = (IntegerLiteralExpressionNode)neg.Operand;
            Assert.AreEqual(4L, inl.Value);
        }

        [Test]
        public void UnaryNegationWithBracketedOperandTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-(4 + 6)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<NegationUnaryOperationExpressionNode>(root);

            var neg = (NegationUnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<AdditionBinaryOperationExpressionNode>(neg.Operand);
        }

        [Test]
        public void UnaryNegationPrecedenceTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-4 + 6").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<AdditionBinaryOperationExpressionNode>(root);

            var add = (AdditionBinaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<NegationUnaryOperationExpressionNode>(add.Left);
        }

        [Test]
        public void UnaryNegationFunctionCallTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-Sin(4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<NegationUnaryOperationExpressionNode>(root);

            var neg = (NegationUnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<FunctionCallExpressionNode>(neg.Operand);
        }

        [Test]
        public void ShiftTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x -> y + z").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<BitwiseRightShiftBinaryOperationExpressionNode>(root);

            var shift = (BitwiseRightShiftBinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(shift.Left);
            Assert.IsInstanceOf<AdditionBinaryOperationExpressionNode>(shift.Right);
        }

        [Test]
        public void ExponentiationAssociativeTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x ** y ** z").LexAll()); // x ** (y ** z)
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<ExponentiationBinaryOperationExpressionNode>(root);

            var exp = (ExponentiationBinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.Left);
            Assert.IsInstanceOf<ExponentiationBinaryOperationExpressionNode>(exp.Right);
        }

        [Test]
        public void OtherAssociativeTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x * y * z").LexAll()); // (x * y) * z
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<MultiplicationBinaryOperationExpressionNode>(root);

            var exp = (MultiplicationBinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<MultiplicationBinaryOperationExpressionNode>(exp.Left);
            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.Right);
        }
    }
}
