﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
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

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

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

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

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

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));
            Node? root = parser.ParseNextStatement(ref index);

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

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes), new TypeParser(lexemes));

            Assert.Throws<NotImplementedException>(() => parser.ParseNextStatement(ref index));
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

            Assert.IsInstanceOf<IdentifierExpressionNode>(fcsn.FunctionExpression);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(fcsn.Arguments?[0]);
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
            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(vasn.AssignedValue);
            Assert.AreEqual("x", vasn.Identifier);
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

            Assert.AreEqual(2, block.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block[0]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(block[1]);
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

            Assert.IsInstanceOf<BooleanLiteralExpressionNode>(@while.Condition);
            Assert.AreEqual(1, @while.Statements.Count);
        }
    }
}
