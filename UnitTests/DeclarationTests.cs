using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Syntactical;
using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using NUnit.Framework;

namespace UnitTests
{
    class DeclarationTests
    {
        [Test]
        public void FunctionDeclarationNoParametersNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld()
            {
                Output(""Hello world"");
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(0, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationOneParameterNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As String)
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(1, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationTwoParametersNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As String, y As Int)
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(2, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationNoParametersWithReturnTest()
        {
            const string Code = @"
            Func HelloWorld() As Int
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(0, func.ParameterNodes.Count);
            Assert.NotNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationTwoParametersWithReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As Complex, y As Char) As Int
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(2, func.ParameterNodes.Count);
            Assert.NotNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void ReturnNoExpressionTest()
        {
            const string Code = @"
            Return;
            ";

            ProgramNode? program = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseWholeProgram();
            });

            Assert.NotNull(program);
            Assert.AreEqual(1, program!.TopLevelStatementNodes.Count);
            Assert.IsTrue(program.TopLevelStatementNodes[0] is ReturnStatementNode
            {
                ReturnExpressionNode: null
            });
        }

        [Test]
        public void ReturnWithExpressionTest()
        {
            const string Code = @"
            Return 4;
            ";

            ProgramNode? program = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseWholeProgram();
            });

            Assert.NotNull(program);
            Assert.AreEqual(1, program!.TopLevelStatementNodes.Count);
            Assert.IsTrue(program.TopLevelStatementNodes[0] is ReturnStatementNode
            {
                ReturnExpressionNode: IntegerLiteralExpressionNode
                {
                    Value: 4
                }
            });
        }

        //[Test]
        //public void ReturnMissingSemicolonTest()
        //{
        //    const string Code = @"
        //    Return 4
        //    ";

        //    var e = MyAssert.Error(() =>
        //    {
        //        var lexemes = new Lexer(Code).LexAll();
        //        var parser = new ProgramParser(lexemes, Code);
        //        var p = parser.ParseWholeProgram();
        //        Assert.IsNull(p);
        //    });

        //    Assert.AreEqual(ErrorCode.ExpectedSemicolon, e.ErrorCode);
        //}

        [Test]
        public void ConstDeclarationImplicitTypeTest()
        {
            const string Code = @"
            Const BEST_NUMBER = 4;
            ";

            ConstantDeclarationNode? cnst = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseConstantDeclaration();
            });

            Assert.NotNull(cnst);
            Assert.AreEqual("BEST_NUMBER", cnst!.Identifier);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(cnst.ValueNode);
        }

        [Test]
        public void ConstDeclarationExplicitTypeTest()
        {
            const string Code = @"
            Const BEST_NUMBER As Int = 4;
            ";

            ConstantDeclarationNode? cnst = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseConstantDeclaration();
            });

            Assert.NotNull(cnst);
            Assert.AreEqual("BEST_NUMBER", cnst!.Identifier);
            Assert.IsInstanceOf<IntegerLiteralExpressionNode>(cnst.ValueNode);
            Assert.IsTrue(cnst.TypeSpecNode is IdentifierTypeSpecNode { Identifier: "Int" });
        }

        [Test]
        public void ConstDeclarationNoInitTest()
        {
            const string Code = @"
            Const BEST_NUMBER;
            ";

            MyAssert.Error(Code, Krypton.Analysis.Errors.ErrorCode.LetVariableAndConstMustBeInitialized);
        }

        [Test]
        public void ComplexConstantTest()
        {
            const string Code = @"
            Const Z = 3 + 4i;
            Var z = Z;
            ";

            var c = MyAssert.NoError(Code);

            Assert.IsTrue(c.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
            {
                AssignedExpressionNode: IdentifierExpressionNode
                {
                    IdentifierNode: BoundIdentifierNode
                    {
                        Symbol: ConstantSymbolNode<Complex>
                        {
                            Value:
                            {
                                Real:
                                {
                                    Numerator: 3,
                                    Denominator: 1
                                },
                                Imaginary:
                                {
                                    Numerator: 4,
                                    Denominator: 1
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void ComplexConstantMinusTest()
        {
            const string Code = @"
            Const Z = 3 - 4i;
            Var z = Z;
            ";

            var c = MyAssert.NoError(Code);

            Assert.IsTrue(c.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
            {
                AssignedExpressionNode: IdentifierExpressionNode
                {
                    IdentifierNode: BoundIdentifierNode
                    {
                        Symbol: ConstantSymbolNode<Complex>
                        {
                            Value:
                            {
                                Real:
                                {
                                    Numerator: 3,
                                    Denominator: 1
                                },
                                Imaginary:
                                {
                                    Numerator: -4,
                                    Denominator: 1
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void NegativeComplexConstantTest()
        {
            const string Code = @"
            Const Z = -3 + 4i;
            Var z = Z;
            ";

            var c = MyAssert.NoError(Code);

            Assert.IsTrue(c.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
            {
                AssignedExpressionNode: IdentifierExpressionNode
                {
                    IdentifierNode: BoundIdentifierNode
                    {
                        Symbol: ConstantSymbolNode<Complex>
                        {
                            Value:
                            {
                                Real:
                                {
                                    Numerator: -3,
                                    Denominator: 1
                                },
                                Imaginary:
                                {
                                    Numerator: 4,
                                    Denominator: 1
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void NegativeComplexConstantMinusTest()
        {
            const string Code = @"
            Const Z = -3 - 4i;
            Var z = Z;
            ";

            var c = MyAssert.NoError(Code);

            Assert.IsTrue(c.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
            {
                AssignedExpressionNode: IdentifierExpressionNode
                {
                    IdentifierNode: BoundIdentifierNode
                    {
                        Symbol: ConstantSymbolNode<Complex>
                        {
                            Value:
                            {
                                Real:
                                {
                                    Numerator: -3,
                                    Denominator: 1
                                },
                                Imaginary:
                                {
                                    Numerator: -4,
                                    Denominator: 1
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void OtherComplexConstantTest()
        {
            const string Code = @"
            Const Z = -3 ** 4i;
            ";

            MyAssert.Error(Code, Krypton.Analysis.Errors.ErrorCode.ConstantValueMustBeLiteralOrComplex);
        }

        [Test]
        public void ConstantIllegalInitTest()
        {
            const string Code = @"
            Const Y = 4 ** 5;
            ";

            MyAssert.Error(Code, Krypton.Analysis.Errors.ErrorCode.ConstantValueMustBeLiteralOrComplex);
        }
    }
}
