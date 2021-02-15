using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Syntactical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Framework;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public class ParserTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void SimpleExpressionTest()
        {
            const string Code = "3";

            LexemeCollection lexemes = new Lexer(Code).LexAll();

            Assert.AreEqual(2, lexemes.Count);
            Assert.IsInstanceOf<IntegerLiteralLexeme>(lexemes[0]);
            Assert.IsInstanceOf<EndOfFileLexeme>(lexemes[1]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes, Code).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(3, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void BracketedExpressionTest()
        {
            const string Code = "(4)";

            LexemeCollection lexemes = new Lexer(Code).LexAll();

            Assert.AreEqual(4, lexemes.Count);
            Assert.IsTrue(lexemes[0] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening });
            Assert.IsInstanceOf<IntegerLiteralLexeme>(lexemes[1]);
            Assert.IsTrue(lexemes[2] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing });
            Assert.IsInstanceOf<EndOfFileLexeme>(lexemes[3]);

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes, Code).ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(4, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void IllegalExpressionTest()
        {
            const string Code = "(5";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            var e = MyAssert.Error(() =>
                                   {
                                       ExpressionNode? exp = new ExpressionParser(lexemes, Code).ParseNextExpression(ref index);
                                       Assert.IsNull(exp);
                                   });
            Assert.AreEqual(ErrorCode.ExpectedClosingParenthesis, e.ErrorCode);
        }

        [Test]
        public void SeveralNestedParensTest()
        {
            LexemeCollection lexemes = new Lexer("((6))").LexAll();

            int index = 0;
            ExpressionNode? root = new ExpressionParser(lexemes, "((6))").ParseNextExpression(ref index);
            Assert.IsNotNull(root);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(root);
            Assert.AreEqual(6, ((IntegerLiteralExpressionNode)root!).Value);
        }

        [Test]
        public void ChainResolutionTest()
        {
            BinaryOperationChain chain = new(1, 5); // Arbitrary line and index number
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1, 10));
            chain.AddOperator(new CharacterOperatorLexeme(Operator.Plus, 1, 0));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1, 11));
            chain.AddOperator(new CharacterOperatorLexeme(Operator.Asterisk, 1, 0));
            chain.AddOperand(new IntegerLiteralExpressionNode(4, 1, 12));
            // 4 + 4 * 4 = 20

            ExpressionNode node = chain.Resolve();

            Assert.IsInstanceOf<BinaryOperationExpressionNode>(node);

            BinaryOperationExpressionNode boen = (node as BinaryOperationExpressionNode)!;

            Assert.AreEqual(Operator.Plus, boen.Operator);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(boen.LeftOperandNode);
            Assert.IsTrue(boen.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });
        }

        [Test]
        public void OperationTest()
        {
            const string Code = "3 + 4";

            int index = 0;
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.LeftOperandNode);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.RightOperandNode);

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.RightOperandNode).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithBracketsTest()
        {
            int index = 0;
            const string Code = "(3 + 4)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.LeftOperandNode);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.RightOperandNode);

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.RightOperandNode).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithIndividualBracketsTest()
        {
            int index = 0;
            const string Code = "(3) + (4)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.LeftOperandNode);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.RightOperandNode);

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            long right = ((IntegerLiteralExpressionNode)aboen.RightOperandNode).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void LongerOperationTest()
        {
            int index = 0;
            const string Code = "3 + 4 * 5";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.LeftOperandNode);
            Assert.IsTrue(aboen.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            Assert.AreEqual(3, left);
        }

        [Test]
        public void LongerOperationWithBracketsTest()
        {
            int index = 0;
            const string Code = "(3 + 4 * 5)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var aboen = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(aboen.LeftOperandNode);
            Assert.IsTrue(aboen.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            Assert.AreEqual(3, left);
        }

        [Test]
        public void OperationWithNestedOperationsTest()
        {
            int index = 0;
            const string Code = "(3 + 4) * 5";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(mboen.RightOperandNode);

            long right = ((IntegerLiteralExpressionNode)mboen.RightOperandNode).Value;
            Assert.AreEqual(5, right);

            var aboen = (BinaryOperationExpressionNode)mboen.LeftOperandNode;

            long left = ((IntegerLiteralExpressionNode)aboen.LeftOperandNode).Value;
            right = ((IntegerLiteralExpressionNode)aboen.RightOperandNode).Value;

            Assert.AreEqual(3, left);
            Assert.AreEqual(4, right);
        }

        [Test]
        public void OperationWithSubExpressionsOnBothSidesTest()
        {
            int index = 0;
            const string Code = "(3 + 4) / (5 - 6)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.ForeSlash });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsTrue(mboen.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void IntegerDivisionOperationTest()
        {
            int index = 0;
            const string Code = "(3 + 4) Div (5 - 6)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.DivKeyword });

            var mboen = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(mboen.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.IsTrue(mboen.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void BitwiseOperatorsPrecedenceTest()
        {
            int index = 0;
            const string Code = "3 & 4 + 5";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Ampersand });
        }

        [Test]
        public void RangeTest()
        {
            int index = 3;

            //                   0   1 2 3 4 5 6 7 8
            const string Code = "Var x = 5 | 8 + 9;";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Pipe });

            Assert.AreEqual(index, 8);
        }

        [Test]
        public void LogicalOperatorsTest()
        {
            int index = 0;
            const string Code = "True And False Or False";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.OrKeyword });

            BinaryOperationExpressionNode orOperation = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(orOperation.RightOperandNode);
            Assert.IsTrue(orOperation.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.AndKeyword });
        }

        [Test]
        public void StringTest()
        {
            int index = 0;

            const string Code = @"""x""";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            ExpressionParser parser = new(lexemes, Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<StringLiteralExpressionNode>(root);
        }

        [Test]
        public void IdentifierTest()
        {
            int index = 0;

            const string Code = "x * 4";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(op.LeftOperandNode);

            IdentifierExpressionNode id = (IdentifierExpressionNode)op.LeftOperandNode;

            Assert.AreEqual("x", id.IdentifierNode.Identifier);
        }

        [Test]
        public void MoreComplexIdentifierTest()
        {
            int index = 0;

            const string Code = "x * y & (z - 1)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Ampersand });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;
            Assert.IsTrue(op.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            BinaryOperationExpressionNode op2 = (BinaryOperationExpressionNode)op.LeftOperandNode;
            Assert.DoesNotThrow(() =>
            {
                Assert.AreEqual("x", ((IdentifierExpressionNode)op2.LeftOperandNode).IdentifierNode.Identifier);
            });

            Assert.IsTrue(op.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void BracketedIdentifierTest()
        {
            int index = 0;
            const string Code = "(hello)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<IdentifierExpressionNode>(root);
        }

        [Test]
        public void ErrorIdentifierTest()
        {
            int index = 0;
            const string Code = "(hello +";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);

            var e = MyAssert.Error(() => parser.ParseNextExpression(ref index));
            Assert.AreEqual(ErrorCode.ExpectedExpressionTerm, e.ErrorCode);
        }

        [Test]
        public void ErrorTest()
        {
            int index = 0;
            const string Code = "(4;";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);

            var e = MyAssert.Error(() => parser.ParseNextExpression(ref index));
            Assert.AreEqual(1, e.LineNumber);
            Assert.AreEqual(ErrorCode.ExpectedClosingParenthesis, e.ErrorCode);
        }

        [Test]
        public void FunctionCallTest()
        {
            int index = 0;
            const string Code = "Output()";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpressionNode);
            Assert.AreEqual(0, call.ArgumentNodes.Count);
        }

        [Test]
        public void FunctionCallWithOneArgumentTest()
        {
            int index = 0;
            const string Code = "Output(4)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpressionNode);
            Assert.NotNull(call.ArgumentNodes);
            Assert.AreEqual(1, call.ArgumentNodes!.Count);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(call.ArgumentNodes[0]);
        }

        [Test]
        public void FunctionCallWithTwoArgumentsTest()
        {
            int index = 0;
            const string Code = @"Output(4, ""xyz"")";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpressionNode);
            Assert.NotNull(call.ArgumentNodes);
            Assert.AreEqual(2, call.ArgumentNodes!.Count);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(call.ArgumentNodes[0]);
            Assert.IsInstanceOf<StringLiteralExpressionNode>(call.ArgumentNodes[1]);
        }

        [Test]
        public void FunctionCallWithErrorTest()
        {
            int index = 0;
            const string Code = "Output(4, )";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);

            var e = MyAssert.Error(() => parser.ParseNextExpression(ref index));
            Assert.AreEqual(ErrorCode.UnexpectedExpressionTerm, e.ErrorCode);
            Assert.AreEqual(1, e.LineNumber);
        }

        [Test]
        public void FunctionCallWithNestedExpressionAsArgumentTest()
        {
            int index = 0;
            const string Code = "Output(4 + 5 * 6)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpressionNode);
            Assert.NotNull(call.ArgumentNodes);
            Assert.AreEqual(1, call.ArgumentNodes!.Count);
            Assert.IsTrue(call.ArgumentNodes[0] is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }


        [Test]
        public void NestedFunctionCallTest()
        {
            int index = 0;
            const string Code = "Output(4) + 8";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            BinaryOperationExpressionNode op = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<FunctionCallExpressionNode>(op.LeftOperandNode);
        }

        [Test]
        public void ChainedFunctionCallsTest()
        {
            int index = 0;
            const string Code = "Output(4)(True)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallExpressionNode>(root);

            FunctionCallExpressionNode call = (FunctionCallExpressionNode)root!;

            Assert.IsInstanceOf<FunctionCallExpressionNode>(call.FunctionExpressionNode);
        }

        [Test]
        public void AndTest()
        {
            int index = 0;
            const string Code = "x And y";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.AndKeyword });
        }

        [Test]
        public void UnaryNegationTest()
        {
            int index = 0;
            const string Code = "-4";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(neg.OperandNode);

            var inl = (IntegerLiteralExpressionNode)neg.OperandNode;
            Assert.AreEqual(4L, inl.Value);
        }

        [Test]
        public void UnaryNegationWithBracketedOperandTest()
        {
            int index = 0;
            const string Code = "-(4 + 6)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
            Assert.IsTrue(neg.OperandNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void UnaryNegationPrecedenceTest()
        {
            int index = 0;
            const string Code = "-4 + 6";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Plus });

            var add = (BinaryOperationExpressionNode)root!;
            Assert.IsTrue(add.LeftOperandNode is UnaryOperationExpressionNode { Operator: Operator.Minus });
        }

        [Test]
        public void UnaryNegationFunctionCallTest()
        {
            int index = 0;
            const string Code = "-Sin(4)";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is UnaryOperationExpressionNode { Operator: Operator.Minus });

            var neg = (UnaryOperationExpressionNode)root!;
            Assert.IsInstanceOf<FunctionCallExpressionNode>(neg.OperandNode);
        }

        [Test]
        public void ShiftTest()
        {
            int index = 0;
            const string Code = "x -> y + z";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code);
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.SingleRightArrow });

            var shift = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(shift.LeftOperandNode);
            Assert.IsTrue(shift.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void ExponentiationAssociativeTest()
        {
            int index = 0;
            const string Code = "x ** y ** z";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code); // x ** (y ** z)
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.DoubleAsterisk });

            var exp = (BinaryOperationExpressionNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.LeftOperandNode);
            Assert.IsTrue(exp.RightOperandNode is BinaryOperationExpressionNode { Operator: Operator.DoubleAsterisk });
        }

        [Test]
        public void OtherAssociativeTest()
        {
            int index = 0;
            const string Code = "x * y * z";
            ExpressionParser parser = new(new Lexer(Code).LexAll(), Code); // (x * y) * z
            ExpressionNode? root = parser.ParseNextExpression(ref index);

            Assert.NotNull(root);
            Assert.IsTrue(root is BinaryOperationExpressionNode { Operator: Operator.Asterisk });

            var exp = (BinaryOperationExpressionNode)root!;

            Assert.IsTrue(exp.LeftOperandNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });
            Assert.IsInstanceOf<IdentifierExpressionNode>(exp.RightOperandNode);
        }

        [Test]
        public void PropertyGetTest()
        {
            const string Code = @"""x"".Length";

            var r = MyAssert.NoError(() =>
            {
                Lexer lxr = new(Code);
                LexemeCollection lxms = lxr.LexAll();
                ExpressionParser prsr = new(lxms, Code);

                int index = 0;

                return prsr.ParseNextExpression(ref index);
            });

            Assert.IsTrue(r is PropertyGetExpressionNode
            {
                ExpressionNode: StringLiteralExpressionNode
                {
                    Value: "x"
                },
                PropertyIdentifierNode:
                {
                    Identifier: "Length"
                }
            });
        }
    }
}
