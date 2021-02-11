using Krypton.Analysis;
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

            Assert.NotNull(vdsn.TypeSpecNode);
            Assert.NotNull(vdsn.AssignedExpressionNode);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.TypeSpecNode);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedExpressionNode);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedExpressionNode!;

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

            Assert.IsNull(vdsn.TypeSpecNode);
            Assert.NotNull(vdsn.AssignedExpressionNode);

            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(vdsn.AssignedExpressionNode);

            var ilen = (IntegerLiteralExpressionNode)vdsn.AssignedExpressionNode!;

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

            Assert.NotNull(vdsn.TypeSpecNode);
            Assert.IsNull(vdsn.AssignedExpressionNode);

            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdsn.TypeSpecNode);
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

            Assert.IsNull(vdsn.TypeSpecNode);
            Assert.NotNull(vdsn.AssignedExpressionNode);

            Assert.IsTrue(vdsn.AssignedExpressionNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
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

        [Test]
        public void IfTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.IsInstanceOf<BinaryOperationExpressionNode>(ifStatement.ConditionNode);
            Assert.AreEqual(1, ifStatement.StatementNodes.Count);

            Assert.IsNull(ifStatement.ElsePartNode);
            Assert.AreEqual(0, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IfElseTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.NotNull(ifStatement.ElsePartNode);
            Assert.AreEqual(0, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IfElseIfTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else If x == 5
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.IsNull(ifStatement.ElsePartNode);
            Assert.AreEqual(1, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IfElseIfElseTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else If x == 5
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }
            Else
            {
                Output(""hmm"");
            }";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.NotNull(ifStatement.ElsePartNode);
            Assert.AreEqual(1, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IfMultipleElseIfsTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else If x == 5
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }
            Else If x == 6
            {
                Output(""hmm"");
            }";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.IsNull(ifStatement.ElsePartNode);
            Assert.AreEqual(2, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IfWithTrailingStatementsTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else If x == 5
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }
            Else
            {
                Output(""hmm"");
            }
            Output(""Hello world"");";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(3, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<IfStatementNode>(program.TopLevelStatementNodes[1]);

            var ifStatement = (IfStatementNode)program.TopLevelStatementNodes[1];

            Assert.NotNull(ifStatement.ElsePartNode);
            Assert.AreEqual(1, ifStatement.ElseIfPartNodes.Count);
        }

        [Test]
        public void IllegalIfTest()
        {
            const string Code = @"
            Var x = 4;
            If x == 4
            {
                Output(""works"");
            }
            Else x == 5
            {
                Output(""time space continuum broken; 4 != 4??? owo"");
            }";

            var e = MyAssert.Error(() =>
            {
                LexemeCollection lexemes = new Lexer(Code).LexAll();
                ProgramParser parser = new ProgramParser(lexemes, Code);
                ProgramNode? program = parser.ParseWholeProgram();

                Assert.IsNull(program);
            });

            Assert.AreEqual(ErrorCode.ExpectedOpeningBrace, e.ErrorCode);
        }

        [Test]
        public void ForWhileTest()
        {
            const string Code = @"
            For Var i = 0 While i < 10
            {
                Output(""Test"");
            }
            ";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(1, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<ForStatementNode>(program.TopLevelStatementNodes[0]);

            var forStmt = (ForStatementNode)program.TopLevelStatementNodes[0];

            Assert.NotNull(forStmt.ConditionNode);
            Assert.IsNull(forStmt.WithExpressionNode);
            Assert.IsTrue(forStmt.DeclaresNew);
            Assert.AreEqual("i", forStmt.VariableIdentifier);
            Assert.AreEqual(1, forStmt.StatementNodes.Count);

            Assert.IsTrue(forStmt.ConditionNode is BinaryOperationExpressionNode { Operator: Operator.LessThan });
        }

        [Test]
        public void ForWithTest()
        {
            const string Code = @"
            For Var i = 0 With i = i + 1
            {
                Output(""Test"");
            }
            ";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(1, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<ForStatementNode>(program.TopLevelStatementNodes[0]);

            var forStmt = (ForStatementNode)program.TopLevelStatementNodes[0];

            Assert.IsNull(forStmt.ConditionNode);
            Assert.NotNull(forStmt.WithExpressionNode);
            Assert.IsTrue(forStmt.DeclaresNew);
            Assert.AreEqual("i", forStmt.VariableIdentifier);
            Assert.AreEqual(1, forStmt.StatementNodes.Count);

            Assert.IsTrue(forStmt.WithExpressionNode is BinaryOperationExpressionNode { Operator: Operator.Plus });
        }

        [Test]
        public void ForWhileWithTest()
        {
            const string Code = @"
            For Var i = 0 While i <= 20 With i = i * 2
            {
                Output(""Test"");
            }
            ";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(1, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<ForStatementNode>(program.TopLevelStatementNodes[0]);

            var forStmt = (ForStatementNode)program.TopLevelStatementNodes[0];

            Assert.NotNull(forStmt.ConditionNode);
            Assert.NotNull(forStmt.WithExpressionNode);
            Assert.IsTrue(forStmt.DeclaresNew);
            Assert.AreEqual("i", forStmt.VariableIdentifier);
            Assert.AreEqual(1, forStmt.StatementNodes.Count);

            Assert.IsTrue(forStmt.ConditionNode is BinaryOperationExpressionNode { Operator: Operator.LessThanEquals });
            Assert.IsTrue(forStmt.WithExpressionNode is BinaryOperationExpressionNode { Operator: Operator.Asterisk });
        }

        [Test]
        public void ForNeitherWhileNorWithTest()
        {
            const string Code = @"
            For Var i = 0
            {
                Output(""Test"");
            }
            ";

            var e = MyAssert.Error(() =>
            {
                LexemeCollection lexemes = new Lexer(Code).LexAll();
                ProgramParser parser = new ProgramParser(lexemes, Code);
                ProgramNode? program = parser.ParseWholeProgram();

                Assert.IsNull(program);
            });

            Assert.AreEqual(ErrorCode.ForNeitherWhileNorWith, e.ErrorCode);
        }

        [Test]
        public void ForIllegalConditionTest()
        {
            const string Code = @"
            For Var i = 0 While i != 4
            {
                Output(""Test"");
            }
            ";

            var e = MyAssert.Error(() =>
            {
                LexemeCollection lexemes = new Lexer(Code).LexAll();
                ProgramParser parser = new ProgramParser(lexemes, Code);
                ProgramNode? program = parser.ParseWholeProgram();

                Assert.IsNull(program);
            });

            Assert.AreEqual(ErrorCode.ForConditionHasToBeTrueOrComparisonWithIterationVariable, e.ErrorCode);
        }

        [Test]
        public void ForIllegalWithPartTest()
        {
            const string Code = @"
            For Var i = 0 With i + 1
            {
                Output(""Test"");
            }
            ";

            var e = MyAssert.Error(() =>
            {
                LexemeCollection lexemes = new Lexer(Code).LexAll();
                ProgramParser parser = new ProgramParser(lexemes, Code);
                ProgramNode? program = parser.ParseWholeProgram();

                Assert.IsNull(program);
            });

            Assert.AreEqual(ErrorCode.ForWithPartHasToAssignIterationVariable, e.ErrorCode);
        }

        [Test]
        public void ForIllegalDeclarationPartTest()
        {
            const string Code = @"
            For Var i With i + 1
            {
                Output(""Test"");
            }
            ";

            var e = MyAssert.Error(() =>
            {
                LexemeCollection lexemes = new Lexer(Code).LexAll();
                ProgramParser parser = new ProgramParser(lexemes, Code);
                ProgramNode? program = parser.ParseWholeProgram();

                Assert.IsNull(program);
            });

            Assert.AreEqual(ErrorCode.NewVariableInForWithoutDefaultValue, e.ErrorCode);
        }

        [Test]
        public void ForExistingVariablePartTest()
        {
            const string Code = @"
            Var i = 0;
            For i With i = i + 1
            {
                Output(""Test"");
            }
            ";

            LexemeCollection lexemes = new Lexer(Code).LexAll();
            ProgramParser parser = new ProgramParser(lexemes, Code);
            ProgramNode? program = parser.ParseWholeProgram();

            Assert.NotNull(program);
            Assert.AreEqual(2, program!.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<ForStatementNode>(program.TopLevelStatementNodes[1]);

            var forStmt = (ForStatementNode)program.TopLevelStatementNodes[1];

            Assert.IsFalse(forStmt.DeclaresNew);
        }
    }
}
