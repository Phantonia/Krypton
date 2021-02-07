using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Syntactical;
using Krypton.Framework;
using NUnit.Framework;
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
            const string Code = "Var x As Int = 5;";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            Assert.AreEqual(8, lexemes.Count);
            Assert.IsInstanceOf<IdentifierLexeme>(lexemes[1]);

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValueNode);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.Type);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedValueNode);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedValueNode!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithValueTest()
        {
            const string Code = "Var x = 5;";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValueNode);

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedValueNode);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedValueNode!;

            Assert.AreEqual(5, ilen.Value);
        }

        [Test]
        public void VariableDeclarationWithTypeTest()
        {
            const string Code = "Var x As Int;";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.NotNull(vdsn.Type);
            Assert.IsNull(vdsn.AssignedValueNode);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.Type);
        }

        [Test]
        public void VariableDeclarationMoreComplexExpressionTest()
        {
            const string Code = "Var x = 5 + 6 * 9;";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
            Node? root = parser.ParseNextStatement(ref index);

            Assert.NotNull(root);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(root);

            var vdsn = (VariableDeclarationStatementNode)root!;

            Assert.IsNull(vdsn.Type);
            Assert.NotNull(vdsn.AssignedValueNode);

            Assert.IsTrue(vdsn.AssignedValueNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void IllegalVariableDeclarationTest()
        {
            const string Code = "Var x As Int";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);

            var e = MyAssert.Error(() => parser.ParseNextStatement(ref index));
            Assert.AreEqual(ErrorCode.ExpectedEqualsOrSemicolon, e.ErrorCode);
        }

        [Test]
        public void FunctionCallTest()
        {
            const string Code = "Output(4);";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
            const string Code = "x = True;";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);

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
            const string Code = "Var x = 4; Output(x); x = 6; Output(x);";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);

            List<StatementNode> statements = new();

            while (true)
            {
                if (lexemes[index] is EndOfFileLexeme)
                {
                    break;
                }

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
            const string Code = "Block { Output(4); Output(6); }";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
            const string Code = "While True { Output(4.9); }";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
            const string Code = @"Block
{
    Output(4.9);
    Block
    {
        Output(True);
    }
    Var x As Int;
}";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            int index = 0;

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
            const string Code = @"While a + 1
                {
                    While b / 4
                    {
                        Output(c);
                    }
                }";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
            const string Code = @"Block
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
                }";
            LexemeCollection lexemes = new Lexer(Code).LexAll();

            StatementParser parser = new(lexemes, new ExpressionParser(lexemes, Code), new TypeParser(lexemes), Code);
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
