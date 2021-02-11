using Krypton.Analysis;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;
using Krypton.Analysis.Syntactical;
using Krypton.Framework;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class BindingTests
    {
        [Test]
        public void SimpleBindingTest()
        {
            const string Code =
            @"
            Var x = 1;
            x = 2;
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree!.Program.TopLevelStatementNodes[0]);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(tree.Program.TopLevelStatementNodes[1]);

            var decl = (VariableDeclarationStatementNode)tree!.Program.TopLevelStatementNodes[0];
            var assg = (VariableAssignmentStatementNode)tree!.Program.TopLevelStatementNodes[1];

            Assert.IsInstanceOf<BoundIdentifierNode>(decl.VariableIdentifierNode);
            Assert.IsInstanceOf<BoundIdentifierNode>(assg.VariableIdentifierNode);

            var var1 = ((BoundIdentifierNode)decl.VariableIdentifierNode).Symbol;
            var var2 = ((BoundIdentifierNode)assg.VariableIdentifierNode).Symbol;

            Assert.IsTrue(ReferenceEquals(var1, var2));
        }

        [Test]
        public void IllegalBindingTest()
        {
            const string Code =
            @"
            Var x = 1;
            y = 2;
            ";

            var e = MyAssert.Error(Code);
            Assert.AreEqual(ErrorCode.CantAssignUndeclaredVariable, e.ErrorCode);
            Assert.AreEqual(3, e.LineNumber);
            Assert.AreEqual(1, e.Column);

            //MyAssert.Error(Code,
            //               e =>
            //               {
            //                   Assert.AreEqual(ErrorCode.CantAssignUndeclaredVariable, e.ErrorCode);
            //                   Assert.AreEqual(3, e.LineNumber);
            //                   Assert.AreEqual(1, e.Column);
            //               });
        }

        [Test]
        public void MultibleVariablesBindingTest()
        {
            const string Code =
            @"
            Var x = 1; ... decl1
            Var y = x; ... decl2
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);

            var decl1 = tree!.Program.TopLevelStatementNodes[0] as VariableDeclarationStatementNode;
            var decl2 = tree!.Program.TopLevelStatementNodes[1] as VariableDeclarationStatementNode;

            Assert.NotNull(decl1);
            Assert.NotNull(decl2);

            var varX = decl1!.VariableIdentifierNode as BoundIdentifierNode;
            var varY = decl2!.VariableIdentifierNode as BoundIdentifierNode;

            Assert.NotNull(varX);
            Assert.NotNull(varY);

            var idX = decl2.AssignedExpressionNode as IdentifierExpressionNode;

            Assert.NotNull(idX);

            var boundIdX = idX!.IdentifierNode as BoundIdentifierNode;

            Assert.IsTrue(ReferenceEquals(varX!.Symbol, boundIdX!.Symbol));
        }

        [Test]
        public void BlockBindingTest()
        {
            const string Code =
            @"
            Var x = 1;
            Block
            {
                Var y = x;
            }
            y = x;
            ";

            var e = MyAssert.Error(Code);
            Assert.AreEqual(ErrorCode.CantAssignUndeclaredVariable, e.ErrorCode);
            Assert.AreEqual(7, e.LineNumber);
            Assert.AreEqual(1, e.Column);
        }

        [Test]
        public void FrameworkTypeBindingTest()
        {
            const string Code =
            @"
            Var x As String;
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(1, tree!.Program.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree.Program.TopLevelStatementNodes[0]);

            var vdecl = (VariableDeclarationStatementNode)tree.Program.TopLevelStatementNodes[0];

            Assert.AreEqual("x", vdecl.VariableIdentifier);
            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdecl.TypeSpecNode);

            var idtype = (IdentifierTypeSpecNode)vdecl.TypeSpecNode!;

            Assert.IsInstanceOf<BoundIdentifierNode>(idtype.IdentifierNode);

            var bdid = (BoundIdentifierNode)idtype.IdentifierNode;

            Assert.IsInstanceOf<FrameworkTypeSymbolNode>(bdid.Symbol);

            var bitsn = (FrameworkTypeSymbolNode)bdid.Symbol;

            Assert.AreEqual(FrameworkType.String, bitsn.FrameworkType);
        }

        [Test]
        public void FrameworkFuncBindingTest()
        {
            const string Code =
            @"
            Output(""4"");
            ";

            Compilation? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(1, tree!.Program.TopLevelStatementNodes.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(tree.Program.TopLevelStatementNodes[0]);

            var fcsn = (FunctionCallStatementNode)tree.Program.TopLevelStatementNodes[0];

            Assert.IsInstanceOf<IdentifierExpressionNode>(fcsn.FunctionExpressionNode);

            var idex = (IdentifierExpressionNode)fcsn.FunctionExpressionNode;

            Assert.IsInstanceOf<BoundIdentifierNode>(idex.IdentifierNode);

            var bdid = (BoundIdentifierNode)idex.IdentifierNode;

            Assert.IsInstanceOf<FrameworkFunctionSymbolNode>(bdid.Symbol);

            var func = (FrameworkFunctionSymbolNode)bdid.Symbol;

            Assert.AreEqual("Output", func.Identifier);
            Assert.AreEqual("console.log(uwu)", func.Generator(new[] { "uwu" }));
        }

        [Test]
        public void RepeatingVarIdentifierTest()
        {
            const string Code =
            @"
            Var x = 5;
            Var x = ""uwu"";
            ";

            var e = MyAssert.Error(Code);
            Assert.AreEqual(ErrorCode.CantRedeclareVariable, e.ErrorCode);
            Assert.AreEqual(ErrorMessages.Messages[ErrorCode.CantRedeclareVariable], e.Message);
            Assert.AreEqual(Code, e.EntireCode);
            Assert.AreEqual(3, e.LineNumber);
            Assert.AreEqual(5, e.Column);
        }

        [Test]
        public void LegalRepeatingVarIdentifierTest()
        {
            const string Code =
            @"
            Block
            {
                Var x = 4;
            }
            Var x = ""uwu"";
            ";

            Assert.DoesNotThrow(() => Analyser.Analyse(Code));
        }

        [Test]
        public void FrameworkIdentifierAndLocalIdentifierClashTest()
        {
            const string Code =
            @"
            Var Output = 5;
            Output(4);
            ";

            // this shows that shadowing worked and "Output" refers
            // to the variable and not the function
            MyAssert.Error(Code, ErrorCode.CanOnlyCallFunctions);
        }

        [Test]
        public void IfBindingTest()
        {
            const string Code =
            @"
            Var x = 4;
            If x == 4
            {
                Output(""*sigh* everything alright"");
            }
            Else
            {
                Output(""Help, 4 != 4 ;-;"");
            }
            ";

            Compilation tree = MyAssert.NoError(Code);

            Assert.AreEqual(2, tree.Program.TopLevelStatementNodes.Count);

            Assert.IsTrue(tree.Program.TopLevelStatementNodes[1] is IfStatementNode
            {
                ConditionNode: BinaryOperationExpressionNode
                {
                    LeftOperandNode: IdentifierExpressionNode
                    {
                        IdentifierNode: BoundIdentifierNode
                        {
                            Symbol: LocalVariableSymbolNode
                            {
                                Identifier: "x"
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void ForVarBindingTest()
        {
            const string Code =
            @"
            For Var i = 0 While i < 10
            {
                Output(""This is printed 10 times ^^"");
            }
            ";

            Compilation? tree = MyAssert.NoError(Code);

            Assert.AreEqual(1, tree!.Program.TopLevelStatementNodes.Count);

            Assert.IsTrue(tree.Program.TopLevelStatementNodes[0] is ForStatementNode
            {
                VariableIdentifierNode: BoundIdentifierNode
                {
                    Symbol: LocalVariableSymbolNode
                    {
                        Identifier: "i"
                    }
                },
                ConditionNode: BinaryOperationExpressionNode
                {
                    LeftOperandNode: IdentifierExpressionNode
                    {
                        IdentifierNode: BoundIdentifierNode
                        {
                            Symbol: LocalVariableSymbolNode
                            {
                                Identifier: "i"
                            }
                        }
                    }
                }
            });
        }
    }
}
