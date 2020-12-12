using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.Grammatical.Expressions;
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
            Assert.IsAssignableFrom<RealDivisionBinaryOperationExpressionNode>(root);

            var mboen = (RealDivisionBinaryOperationExpressionNode)root!;

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
    }
}
