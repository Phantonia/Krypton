using Krypton.Analysis;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;
using Krypton.Analysis.Syntactical;
using Krypton.Framework;
using NUnit.Framework;
using System;

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

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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

            var idX = decl2.AssignedValue as IdentifierExpressionNode;

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

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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
            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdecl.Type);

            var idtype = (IdentifierTypeSpecNode)vdecl.Type!;

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

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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

            Lexer lexer = new(Code);
            LexemeCollection lexemes = lexer.LexAll();

            ProgramParser parser = new(lexemes);
            Compilation? tree = new(parser.ParseWholeProgram()!);

            if (tree != null)
            {
                Binder binder = new(tree);
                bool success = binder.PerformBinding();

                if (!success)
                {
                    tree = null;
                }
            }

            Assert.NotNull(tree);

            var fcsn = (FunctionCallStatementNode)tree!.Program.TopLevelStatementNodes[1];
            var fnex = (IdentifierExpressionNode)fcsn.FunctionExpressionNode;
            var bdid = (BoundIdentifierNode)fnex.IdentifierNode;

            Assert.IsInstanceOf<LocalVariableSymbolNode>(bdid.Symbol);
        }
    }
}
