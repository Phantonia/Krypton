using Krypton.Analysis;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Framework;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public sealed class TypeTests
    {
        [Test]
        public void LegalVariableDeclarationAndAssignmentTest()
        {
            const string Code = @"
            Var x As Int = 4;
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
        }

        [Test]
        public void IllegalVariableDeclarationAndAssignmentTest()
        {
            const string Code = @"
            Var x As Int = True;
            ";

            var e = MyAssert.Error(Code);
            Assert.AreEqual(ErrorCode.CantConvertType, e.ErrorCode);
        }

        [Test]
        public void IntTypeInferenceTest()
        {
            (string code, FrameworkType type)[] literalTests =
            {
                (@"""xyz""", FrameworkType.String),
                ("4", FrameworkType.Int),
                ("3.14", FrameworkType.Rational),
                ("4i", FrameworkType.Complex),
                ("True", FrameworkType.Bool),
                ("'a'", FrameworkType.Char)
            };

            foreach (var literal in literalTests)
            {
                string code = $"Var x = {literal.code};";

                Compilation? tree = Analyser.Analyse(code);

                Assert.NotNull(tree);
                Assert.IsTrue(tree!.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
                {
                    VariableIdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                        {
                            TypeNode: FrameworkTypeSymbolNode
                            {
                                FrameworkType: var biType
                            }
                        }
                    }
                } && biType == literal.type);
            }
        }

        [Test]
        public void LegalOutputCallTest()
        {
            const string Code = @"
            Output(""Hello world"");
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
        }

        [Test]
        public void IllegalOutputCallTest()
        {
            string[] illegalCodes =
            {
                @"Output(True);",
                @"Output();",
                @"Output(""x"", ""y"");"
            };

            foreach (string code in illegalCodes)
            {
                MyAssert.Error(code);
            }
        }

        [Test]
        public void IllegalOutputExpressionTest()
        {
            const string Code = @"
            Var x = Output(""xyz"");
            ";

            MyAssert.Error(Code, ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression);
        }

        [Test]
        public void LegalReAssignmentTest()
        {
            const string Code = @"
            Var x As Bool = True;
            x = False;
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
        }

        [Test]
        public void IllegalReAssignmentTest()
        {
            const string Code = @"
            Var x As Complex = 4i;
            x = 'v';
            ";

            MyAssert.Error(Code, ErrorCode.CantConvertType);
        }

        [Test]
        public void LegalOperatorsTest()
        {
            const string Code = @"
            Var x = (4 + 5) * 6;
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.IsTrue(tree!.Program.TopLevelStatementNodes[0] is VariableDeclarationStatementNode
            {
                VariableIdentifierNode: BoundIdentifierNode
                {
                    Symbol: VariableSymbolNode
                    {
                        TypeNode: FrameworkTypeSymbolNode
                        {
                            FrameworkType: FrameworkType.Int
                        }
                    }
                }
            });
        }

        [Test]
        public void IllegalOperatorsTest()
        {
            const string Code = @"
            Var x = ""y"" / 4;
            ";

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
        }

        [Test]
        public void IllegalNestedOperatorsTest()
        {
            const string Code = @"
            Var x = 4 ** (""a"" + ""b"");
            ";

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
        }

        [Test]
        public void ForIntTypeTest()
        {
            const string Code = @"
            For Var i = 0 While i < 10
            { }";

            Compilation compilation = MyAssert.NoError(Code);

            if (compilation.Program.TopLevelStatementNodes[0] is not ForStatementNode
                {
                    VariableIdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                        {
                            TypeNode: FrameworkTypeSymbolNode
                            {
                                FrameworkType: FrameworkType frameworkType
                            }
                        }
                    }
                })
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(FrameworkType.Int, frameworkType);
        }

        [Test]
        public void ForRationalTypeTest()
        {
            const string Code = @"
            For Var i = 0.0 While i < 10.0
            { }";

            Compilation compilation = MyAssert.NoError(Code);

            if (compilation.Program.TopLevelStatementNodes[0] is not ForStatementNode
                {
                    VariableIdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                        {
                            TypeNode: FrameworkTypeSymbolNode
                            {
                                FrameworkType: FrameworkType frameworkType
                            }
                        }
                    }
                })
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(FrameworkType.Rational, frameworkType);
        }

        [Test]
        public void ForComplexTypeTest()
        {
            const string Code = @"
            For Var i = 0i With i = 2i ... this code makes no sense, but hey, unit tests \o/
            { }";

            Compilation compilation = MyAssert.NoError(Code);

            if (compilation.Program.TopLevelStatementNodes[0] is not ForStatementNode
                {
                    VariableIdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                        {
                            TypeNode: FrameworkTypeSymbolNode
                            {
                                FrameworkType: FrameworkType frameworkType
                            }
                        }
                    }
                })
            {
                Assert.Fail();
                return;
            }

            Assert.AreEqual(FrameworkType.Complex, frameworkType);
        }

        [Test]
        public void IllegalForStringTypeTest()
        {
            const string Code = @"
            For Var i = ""str"" While True
            { }";

            MyAssert.Error(Code, ErrorCode.ForIterationVariableHasToBeNumberType);
        }

        [Test]
        public void IfConditionTest()
        {
            const string Code = @"
            If 4 == 4 { }
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void IllegalIfConditionTest()
        {
            const string Code = @"
            If 4i { }";

            MyAssert.Error(Code, ErrorCode.CantConvertType);
        }

        [Test]
        public void LegalReturnTest()
        {
            const string Code = @"
            Func X() As Int
            {
                Return 4096;
            }
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void LegalNoReturnValueTest()
        {
            const string Code = @"
            Func X()
            {
                Return;
            }
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void IllegalNoReturnValueTest()
        {
            const string Code = @"
            Func X() As String
            {
                Return;
            }
            ";

            MyAssert.Error(Code, ErrorCode.ReturnedNoValueEvenThoughFunctionShouldReturn);
        }

        [Test]
        public void IllegalReturnValueTest()
        {
            const string Code = @"
            Func X()
            {
                Return 'x';
            }
            ";

            MyAssert.Error(Code, ErrorCode.ReturnedValueEvenThoughFunctionDoesNotHaveReturnType);
        }

        [Test]
        public void IllegalReturnCantConvertTest()
        {
            const string Code = @"
            Func X() As Complex
            {
                Return 'x';
            }
            ";

            MyAssert.Error(Code, ErrorCode.CantConvertType);
        }

        [Test]
        public void ConstTypeTest()
        {
            const string Code = @"
            Const X = 4;

            Var x As Int = X;
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void ConstTypeErrorTest()
        {
            const string Code = @"
            Const X = 4;

            Var x As Char = X;
            ";

            MyAssert.Error(Code, ErrorCode.CantConvertType);
        }
    }
}