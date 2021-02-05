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
    }
}
