using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Framework;
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
            Assert.IsInstanceOf<IntegerLiteralLexeme>(lexemes[0]);
            Assert.IsInstanceOf<EndOfFileLexeme>(lexemes[1]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(3, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void BracketedExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("(4)").LexAll();

            Assert.AreEqual(4, lexemes.Count);
            Assert.IsTrue(lexemes[0] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening });
            Assert.IsInstanceOf<IntegerLiteralLexeme>(lexemes[1]);
            Assert.IsTrue(lexemes[2] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing });
            Assert.IsInstanceOf<EndOfFileLexeme>(lexemes[3]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
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
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(6, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void ChainResolutionTest()
        {
            BinaryOperationChainExpressionNode chain = new(1); // Arbitrary line number
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new CharacterOperatorLexeme(Operator.Plus, 1));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            chain.AddOperator(new CharacterOperatorLexeme(Operator.Asterisk, 1));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1));
            // 4 + 4 * 4 = 20

            ExpressionNode node = chain.Resolve();

            Assert.IsInstanceOf<BinaryOperationExpressionNode>(node);

            BinaryOperationExpressionNode boen = (node as BinaryOperationExpressionNode)!;

            Assert.AreEqual(Operator.Plus, boen.Operator);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(boen.Left);
            Assert.IsTrue(boen.Right is BinaryOperationExpressionNode { Operator: Operator.Asterisk });
        }

        [Test]
        public void OperationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("3 + 4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Right);

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Right);

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Right);

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsTrue(aboen.Right is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.Left);
            Assert.IsTrue(aboen.Right is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.Left is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(mboen.Right);

            long right = ((IntegerLiteralExpressionNode)mboen.Right).Value;
            Assert.AreEqual(5, right);

            var aboen = (BinaryOperationExpressionNode)mboen.Left;

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.ForeSlash });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.Left is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsTrue(mboen.Right is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void IntegerDivisionOperationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(3 + 4) Div (5 - 6)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.DivKeyword });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.Left is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsTrue(mboen.Right is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void BitwiseOperatorsPrecedenceTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("3 & 4 + 5").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Ampersand });
        }

        [Test]
        public void RangeTest()
        {
            int index = 3;

            //                                       0   1 2 3 4 5 6 7 8
            ExpressionParser parser = new(new Lexer("Var x = 5 | 8 + 9;").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Pipe });

            Assert.AreEqual(index, 8);
        }

        [Test]
        public void LogicalOperatorsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("True And False Or False").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.OrKeyword });

            BinaryOperationExpressionNode orOperation = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(orOperation.Right);
            Assert.IsTrue(orOperation.Left is BinaryOperationExpressionNode { Operator: Operator.AndKeyword });
        }

        [Test]
        public void StringTest()
        {
            int index = 0;

            LexemeCollection lexemes = new Lexer(@"""x""").LexAll();

            ExpressionParser parser = new(lexemes);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<StringLiteralExpressionNode>(root);
        }

        [Test]
        public void IdentifierTest()
        {
            int index = 0;

            ExpressionParser parser = new(new Lexer("x * 4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(op.Left);

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
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Ampersand });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;
            Assert.IsTrue(op.Left is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            BinaryOperationExpressionNode op2 = (BinaryOperationExpressionNode)op.Left;
            Assert.DoesNotThrow(() =>
            {
                Assert.AreEqual("x", ((IdentifierExpressionNode)op2.Left).IdentifierNode.Identifier);
            });

            Assert.IsTrue(op.Right is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void BracketedIdentifierTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("(hello)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<IdentifierExpressionNode>(root);
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
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.IsNull(call.Arguments);
        }

        [Test]
        public void FunctionCallWithOneArgumentTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(1, call.Arguments!.Count);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(call.Arguments[0]);
        }

        [Test]
        public void FunctionCallWithTwoArgumentsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer(@"Output(4, ""xyz"")").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(2, call.Arguments!.Count);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(call.Arguments[0]);
            Assert.IsInstanceOf<StringLiteralExpressionNode>(call.Arguments[1]);
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
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);
            Assert.NotNull(call.Arguments);
            Assert.AreEqual(1, call.Arguments!.Count);
            Assert.IsTrue(call.Arguments[0] is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }


        [Test]
        public void NestedFunctionCallTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4) + 8").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<FunctionCallExpressionNode>(op.Left);
        }

        [Test]
        public void ChainedFunctionCallsTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("Output(4)(True)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<FunctionCallExpressionNode>(call.FunctionExpression);
        }

        [Test]
        public void AndTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x And y").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.AndKeyword });
        }

        [Test]
        public void UnaryNegationTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-4").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
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
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
            Assert.IsTrue(neg.Operand is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void UnaryNegationPrecedenceTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-4 + 6").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var add = (BinaryOperationExpressionNode)root!;
            Assert.IsTrue(add.Left is UnaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void UnaryNegationFunctionCallTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("-Sin(4)").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<FunctionCallExpressionNode>(neg.Operand);
        }

        [Test]
        public void ShiftTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x -> y + z").LexAll());
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.SingleRightArrow });

            var shift = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(shift.Left);
            Assert.IsTrue(shift.Right is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void ExponentiationAssociativeTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x ** y ** z").LexAll()); // x ** (y ** z)
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.DoubleAsterisk });

            var exp = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.Left);
            Assert.IsTrue(exp.Right is BinaryOperationExpressionNode { Operator: Operator.DoubleAsterisk });
        }

        [Test]
        public void OtherAssociativeTest()
        {
            int index = 0;
            ExpressionParser parser = new(new Lexer("x * y * z").LexAll()); // (x * y) * z
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            var exp = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(exp.Left is BinaryOperationExpressionNode { Operator: Operator.Asterisk });
            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.Right);
        }
    }
}
