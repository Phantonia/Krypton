using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Syntactical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValue);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.Type);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedValue);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedValue!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithValueTest()
        {
            LexemeCollection lexemes = new Lexer("Var x = 5;").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValue);

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedValue);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedValue!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithTypeTest()
        {
            LexemeCollection lexemes = new Lexer("Var x As Int;").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.IsNull(vdsn.AssignedValue);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.Type);
        }

        [Test]
        public void VariableDeclarationMoreComplexExpressionTest()
        {
            LexemeCollection lexemes = new Lexer("Var x = 5 + 6 * 9;").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValue);

            Assert.IsTrue(vdsn.AssignedValue is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void IllegalVariableDeclarationTest()
        {
            LexemeCollection lexemes = new Lexer("Var x As Int").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));

            MyAssert.Throws<NotImplementedException>(() => parser.ParseNextStatement(ref index));
        }

        [Test]
        public void FunctionCallTest()
        {
            LexemeCollection lexemes = new Lexer("Output(4);").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<FunctionCallStatementNode>(root);

            FunctionCallStatementNode fcsn = (FunctionCallStatementNode)root!;

            Assert.IsInstanceOf<IdentifierExpressionNode>(fcsn.FunctionExpressionNode);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(fcsn.ArgumentNodes?[0]);
        }

        [Test]
        public void VariableAssignmentTest()
        {
            LexemeCollection lexemes = new Lexer("x = True;").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));

            StatementNode? node = parser.ParseNextStatement(ref index);

            Assert.NotNull(node);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(node);

            var vasn = (VariableAssignmentStatementNode)node!;
            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(vasn.AssignedExpressionNode);
            Assert.AreEqual("x", vasn.VariableIdentifier);
        }

        [Test]
        public void SeveralStatementsTest()
        {
            LexemeCollection lexemes = new Lexer("Var x = 4; Output(x); x = 6; Output(x);").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));

            List<StatementNode> statements = new();

            while (true)
            {
                StatementNode? nextStatement = parser.ParseNextStatement(ref index);

                if (nextStatement == null)
                {
                    break;
                }

                statements.Add(nextStatement);
            }

            Assert.AreEqual(4, statements.Count);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(statements[0]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(statements[1]);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(statements[2]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(statements[3]);
        }

        [Test]
        public void BlockStatementTest()
        {
            LexemeCollection lexemes = new Lexer("Block { Output(4); Output(6); }").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            StatementNode? statement = parser.ParseNextStatement(ref index);

            Assert.NotNull(statement);
            Assert.IsInstanceOf<BlockStatementNode>(statement);

            var block = (BlockStatementNode)statement!;

            Assert.AreEqual(2, block.StatementNodes.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block.StatementNodes[0]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block.StatementNodes[1]);
        }

        [Test]
        public void WhileStatementTest()
        {
            LexemeCollection lexemes = new Lexer("While True { Output(4.9); }").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            StatementNode? statement = parser.ParseNextStatement(ref index);

            Assert.NotNull(statement);
            Assert.IsInstanceOf<WhileStatementNode>(statement);

            var @while = (WhileStatementNode)statement!;

            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(@while.ConditionNode);
            Assert.AreEqual(1, @while.StatementNodes.Count);
        }

        [Test]
        public void NestedBlocksTest()
        {
            LexemeCollection lexemes = new Lexer(
@"Block
{
    Output(4.9);
    Block
    {
        Output(True);
    }
    Var x As Int;
}").LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            StatementNode? statement = parser.ParseNextStatement(ref index);

            Assert.NotNull(statement);
            Assert.IsInstanceOf<BlockStatementNode>(statement);

            var block = (BlockStatementNode)statement!;

            Assert.AreEqual(3, block.StatementNodes.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block.StatementNodes[0]);
            Assert.IsInstanceOf<BlockStatementNode>(block.StatementNodes[1]);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(block.StatementNodes[2]);

            var nestedBlock = (BlockStatementNode)block.StatementNodes[1];

            Assert.AreEqual(1, nestedBlock.StatementNodes.Count);
        }

        [Test]
        public void NestedWhileLoopsTest()
        {
            LexemeCollection lexemes = new Lexer(
              @"While a + 1
                {
                    While b / 4
                    {
                        Output(c);
                    }
                }").LexAll();

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            int index = 0;
            StatementNode? statement = parser.ParseNextStatement(ref index);

            Assert.NotNull(statement);
            Assert.IsInstanceOf<WhileStatementNode>(statement);

            var @while = (WhileStatementNode)statement!;

            Assert.IsTrue(@while.ConditionNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
            Assert.AreEqual(1, @while.StatementNodes.Count);
            Assert.IsInstanceOf<WhileStatementNode>(@while.StatementNodes[0]);
        }

        [Test]
        public void MassiveNestingTest()
        {
            LexemeCollection lexemes = new Lexer(
              @"Block
                {
                    Block { }
                    While b / 4
                    {
                        Output(c);
                        Block
                        {
                            y = 6;
                        }
                    }
                    V();
                }").LexAll();

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            int index = 0;
            StatementNode? statement = parser.ParseNextStatement(ref index);

            Assert.NotNull(statement);
            Assert.IsInstanceOf<BlockStatementNode>(statement);

            var block = (BlockStatementNode)statement!;

            Assert.AreEqual(3, block.StatementNodes.Count);
            Assert.IsInstanceOf<BlockStatementNode>(block.StatementNodes[0]);

            var block_block = (BlockStatementNode)block.StatementNodes[0];
            Assert.AreEqual(0, block_block.StatementNodes.Count);

            var block_while = (WhileStatementNode)block.StatementNodes[1];
            Assert.IsTrue(block_while.ConditionNode is BinaryOperationExpressionNode { Operator: Operator.ForeSlash });
            Assert.AreEqual(2, block_while.StatementNodes.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block_while.StatementNodes[0]);
        }
    }
}
