using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
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
            Assert.IsAssignableFrom<ParenthesisOpeningLexeme>(lexemes[0]);
            Assert.IsAssignableFrom<IntegerLiteralLexeme>(lexemes[1]);
            Assert.IsAssignableFrom<ParenthesisClosingLexeme>(lexemes[2]);
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
            BinaryOperationChainNode chain = new(1); // Arbitrary line number
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new PlusLexeme(1));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new AsteriskLexeme(1));
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
    }
}
