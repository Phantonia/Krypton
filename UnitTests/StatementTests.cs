using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class StatementTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void VariableDeclarationWithTypeAndValueTest()
        {
            LexemeCollection lexemes = new Lexer("Var x As Int = 5;").LexAll();

            Assert.AreEqual(8, lexemes.Count);
            Assert.IsInstanceOf<IdentifierLexeme>(lexemes[1]);

            ScriptParser parser = new(lexemes);
            Node? root = parser.ParseNextNode();

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.NotNull(vdsn.Value);

            Assert.IsInstanceOf<IdentifierTypeNode>(vdsn.Type);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.Value);

            var ilen = (IntegerLiteralExpressionNode)vdsn.Value!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithValueTest()
        {
            LexemeCollection lexemes = new Lexer("Var x = 5;").LexAll();
            ScriptParser parser = new(lexemes);
            Node? root = parser.ParseNextNode();

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.Value);

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.Value);

            var ilen = (IntegerLiteralExpressionNode)vdsn.Value!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithTypeTest()
        {
            LexemeCollection lexemes = new Lexer("Var x As Int;").LexAll();
            ScriptParser parser = new(lexemes);
            Node? root = parser.ParseNextNode();

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.IsNull(vdsn.Value);

            Assert.IsInstanceOf<IdentifierTypeNode>(vdsn.Type);
        }

        [Test]
        public void VariableDeclarationMoreComplexExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("Var x = 5 + 6 * 9;").LexAll();
            ScriptParser parser = new(lexemes);
            Node? root = parser.ParseNextNode();

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.Value);

            Assert.IsInstanceOf<AdditionBinaryOperationExpressionNode>(vdsn.Value);
        }

        [Test]
        public void IllegalVariableDeclarationTest()
        {
            LexemeCollection lexemes = new Lexer("Var x As Int").LexAll();
            ScriptParser parser = new(lexemes);
            Node? root = parser.ParseNextNode();

            Assert.IsNull(root);
        }
    }
}
