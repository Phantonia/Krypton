using Krypton.Analysis;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
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

            Compilation tree = Analyser.Analyse(Code);

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

            Compilation tree = Analyser.Analyse(Code);

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

            Compilation tree = Analyser.Analyse(Code);

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

            Compilation tree = Analyser.Analyse(Code);

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
                            Symbol: VariableSymbolNode
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

            Compilation tree = MyAssert.NoError(Code);

            Assert.AreEqual(1, tree!.Program.TopLevelStatementNodes.Count);

            Assert.IsTrue(tree.Program.TopLevelStatementNodes[0] is ForStatementNode
            {
                VariableIdentifierNode: BoundIdentifierNode
                {
                    Symbol: VariableSymbolNode
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
                            Symbol: VariableSymbolNode
                            {
                                Identifier: "i"
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void UserDefinedFunctionSymbolTest()
        {
            const string Code = @"
            Func Sin(x As Rational) As Rational
            { }

            Sin(3.14);
            ";

            Compilation comp = MyAssert.NoError(Code);

            Assert.AreEqual(1, comp.Program.TopLevelStatementNodes.Count);
            Assert.AreEqual(1, comp.Program.Functions.Count);

            Assert.IsTrue(comp.Program.TopLevelStatementNodes[0] is FunctionCallStatementNode
            {
                UnderlyingFunctionCallExpressionNode:
                {
                    FunctionExpressionNode: IdentifierExpressionNode
                    {
                        IdentifierNode: BoundIdentifierNode
                        {
                            Symbol: FunctionSymbolNode
                            {
                                ReturnTypeNode: FrameworkTypeSymbolNode
                                {
                                    FrameworkType: FrameworkType.Rational
                                },
                                ParameterNodes:
                                {
                                    Count: 1
                                }
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void UserDefinedFunctionDoesNotExistTest()
        {
            const string Code = @"
            Func Sin(x As Rational) As Rational
            { }

            Cos(3.14);
            ";

            MyAssert.Error(Code, ErrorCode.CantFindIdentifierInScope);
        }

        [Test]
        public void ParameterBindingTest()
        {
            const string Code = @"
            Func HelloWorld(name As String)
            {
                Output(""Hello "" + name);
            }
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void ParameterDoesNotExistTest()
        {
            const string Code = @"
            Func HelloWorld(name As String)
            { Output(nane); }
            ";

            MyAssert.Error(Code, ErrorCode.CantFindIdentifierInScope);
        }

        [Test]
        public void LetVariableReAssignmentTest()
        {
            const string Code = @"
            Let name = ""Antonia"";
            Output(name);
            name = ""Sandra""; ... who wants to be called Sandra Seriously
            ";

            MyAssert.Error(Code, ErrorCode.CantReAssignReadOnlyVariable);
        }

        [Test]
        public void ForVariableReAssignmentTest()
        {
            const string Code = @"
            For Var i = 0 While i < 10 { i = 4; }
            ";

            MyAssert.Error(Code, ErrorCode.CantReAssignReadOnlyVariable);
        }

        [Test]
        public void ConstantReAssignmentTest()
        {
            const string Code = @"
            Const Name = ""Antonia"";
            Output(Name);
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void ConstantBindingTest()
        {
            const string Code = @"
            Const Name = ""Antonia"";
            Output(Name);
            Name = ""Sandra"";
            ";

            MyAssert.Error(Code, ErrorCode.CantAssignUndeclaredVariable);
        }

        [Test]
        public void ConstantVsVariableTest()
        {
            const string Code = @"
            Const Name = ""Antonia"";
            Var Name = ""Antonia"";
            Output(Name);
            ";

            Compilation comp = MyAssert.NoError(Code);

            if (comp.Program.TopLevelStatementNodes[1] is FunctionCallStatementNode
                {
                    ArgumentNodes: var arguments
                })
            {
                Assert.IsTrue(arguments?[0] is IdentifierExpressionNode
                {
                    IdentifierNode: BoundIdentifierNode
                    {
                        Symbol: VariableSymbolNode
                    }
                });
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void LeaveImplicitLevelTest()
        {
            const string Code = @"
            While True
            {
                If 4 == 4
                {
                    Leave;
                }
            }
            ";

            Compilation comp = MyAssert.NoError(Code);

            var whileStmt = (WhileStatementNode)comp.Program.TopLevelStatementNodes[0];
            var ifStmt = (IfStatementNode)whileStmt.StatementNodes[0];
            var leaveStmt = (LoopControlStatementNode)ifStmt.StatementNodes[0];

            Assert.AreEqual((ushort)1, leaveStmt.Level);
            Assert.AreEqual(LoopControlStatementKind.Leave, leaveStmt.Kind);
            Assert.AreSame(whileStmt, leaveStmt.ControlledLoopNode);
        }

        [Test]
        public void LeaveExplicitLevelTest()
        {
            const string Code = @"
            While True
            {
                If 4 == 4
                {
                    Leave 1;
                }
            }
            ";

            Compilation comp = MyAssert.NoError(Code);

            var whileStmt = (WhileStatementNode)comp.Program.TopLevelStatementNodes[0];
            var ifStmt = (IfStatementNode)whileStmt.StatementNodes[0];
            var leaveStmt = (LoopControlStatementNode)ifStmt.StatementNodes[0];

            Assert.AreEqual((ushort)1, leaveStmt.Level);
            Assert.AreEqual(LoopControlStatementKind.Leave, leaveStmt.Kind);
            Assert.AreSame(whileStmt, leaveStmt.ControlledLoopNode);
        }

        [Test]
        public void LeaveExplicitLevelNestedTest()
        {
            const string Code = @"
            While True
            {
                While True
                {
                    Leave 2;
                }
            }
            ";

            Compilation comp = MyAssert.NoError(Code);

            var whileStmt1 = (WhileStatementNode)comp.Program.TopLevelStatementNodes[0];
            var whileStmt2 = (WhileStatementNode)whileStmt1.StatementNodes[0];
            var leaveStmt = (LoopControlStatementNode)whileStmt2.StatementNodes[0];

            Assert.AreEqual((ushort)2, leaveStmt.Level);
            Assert.AreEqual(LoopControlStatementKind.Leave, leaveStmt.Kind);
            Assert.AreSame(whileStmt1, leaveStmt.ControlledLoopNode);
        }

        [Test]
        public void LeaveExplicitLevelNested2Test()
        {
            const string Code = @"
            While True
            {
                While True
                {
                    Leave 1;
                }
            }
            ";

            Compilation comp = MyAssert.NoError(Code);

            var whileStmt1 = (WhileStatementNode)comp.Program.TopLevelStatementNodes[0];
            var whileStmt2 = (WhileStatementNode)whileStmt1.StatementNodes[0];
            var leaveStmt = (LoopControlStatementNode)whileStmt2.StatementNodes[0];

            Assert.AreEqual((ushort)1, leaveStmt.Level);
            Assert.AreEqual(LoopControlStatementKind.Leave, leaveStmt.Kind);
            Assert.AreSame(whileStmt2, leaveStmt.ControlledLoopNode);
        }

        [Test]
        public void LeaveHighNestingLevelTest()
        {
            const string Code = @"
            While True
            {
                While True
                {
                    For Var i = 0 While True
                    {
                        While i > 0
                        {
                            Leave 4;
                        }
                    }
                }
            }
            ";

            Compilation comp = MyAssert.NoError(Code);

            var whileStmt1 = (WhileStatementNode)comp.Program.TopLevelStatementNodes[0];
            var whileStmt2 = (WhileStatementNode)whileStmt1.StatementNodes[0];
            var whileStmt3 = (ILoopStatementNode)whileStmt2.StatementNodes[0];
            var whileStmt4 = (ILoopStatementNode)whileStmt3.StatementNodes[0];
            var leaveStmt = (LoopControlStatementNode)whileStmt4.StatementNodes[0];

            Assert.AreEqual((ushort)4, leaveStmt.Level);
            Assert.AreEqual(LoopControlStatementKind.Leave, leaveStmt.Kind);
            Assert.AreSame(whileStmt1, leaveStmt.ControlledLoopNode);
        }

        [Test]
        public void LeaveHighNestingLevelNoLoopTest()
        {
            const string Code = @"
            Block { If True { Block { Leave; } } }
            ";

            MyAssert.Error(Code, ErrorCode.LoopControlStatementNotThatDeep);
        }

        [Test]
        public void ContinueTest()
        {
            const string Code = @"
            While True { Continue; }
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void PropertyTest()
        {
            const string Code = @"
            Var i = ""xyz"".Length;
            ";

            MyAssert.NoError(Code);
        }

        [Test]
        public void PropertyDoesNotExistTest()
        {
            const string Code = @"
            Var i = ""xyz"".Lenght; ... oops, typo
            ";

            MyAssert.Error(Code, ErrorCode.PropertyDoesNotExistInType);
        }
    }
}
