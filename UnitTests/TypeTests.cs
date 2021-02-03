using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
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

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
        }

        [Test]
        public void IllegalVariableDeclarationAndAssignmentTest()
        {
            const string Code = @"
            Var x As Int = True;
            ";

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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

                SyntaxTree? tree = Analyser.Analyse(code);

                Assert.NotNull(tree);
                Assert.IsTrue(tree!.Root.TopLevelStatements[0] is VariableDeclarationStatementNode
                {
                    VariableIdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                        {
                            Type: BuiltinTypeSymbolNode
                            {
                                BuiltinType: var biType
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

            SyntaxTree? tree = Analyser.Analyse(Code);

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
                MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(code));
            }
        }

        [Test]
        public void IllegalOutputExpressionTest()
        {
            const string Code = @"
            Var x = Output(""xyz"");
            ";

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
        }

        [Test]
        public void LegalReAssignmentTest()
        {
            const string Code = @"
            Var x As Bool = True;
            x = False;
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
        }

        [Test]
        public void IllegalReAssignmentTest()
        {
            const string Code = @"
            Var x As Complex = 4i;
            x = 'v';
            ";

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
        }

        [Test]
        public void LegalOperatorsTest()
        {
            const string Code = @"
            Var x = (4 + 5) * 6;
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.IsTrue(tree!.Root.TopLevelStatements[0] is VariableDeclarationStatementNode
            {
                VariableIdentifierNode: BoundIdentifierNode
                {
                    Symbol: VariableSymbolNode
                    {
                        Type: BuiltinTypeSymbolNode
                        {
                            BuiltinType: FrameworkType.Int
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
