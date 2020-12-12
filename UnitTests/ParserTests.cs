using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
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
    }
}
