using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
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

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree!.Root.TopLevelStatements[0]);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(tree.Root.TopLevelStatements[1]);

            var decl = (VariableDeclarationStatementNode)tree!.Root.TopLevelStatements[0];
            var assg = (VariableAssignmentStatementNode)tree!.Root.TopLevelStatements[1];

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

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);

            var decl1 = tree!.Root.TopLevelStatements[0] as VariableDeclarationStatementNode;
            var decl2 = tree!.Root.TopLevelStatements[1] as VariableDeclarationStatementNode;

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

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(1, tree!.Root.TopLevelStatements.Count);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree.Root.TopLevelStatements[0]);

            var vdecl = (VariableDeclarationStatementNode)tree.Root.TopLevelStatements[0];

            Assert.AreEqual("x", vdecl.VariableIdentifier);
            Assert.IsInstanceOf<IdentifierTypeSpecNode>(vdecl.Type);

            var idtype = (IdentifierTypeSpecNode)vdecl.Type!;

            Assert.IsInstanceOf<BoundIdentifierNode>(idtype.IdentifierNode);

            var bdid = (BoundIdentifierNode)idtype.IdentifierNode;

            Assert.IsInstanceOf<BuiltinTypeSymbolNode>(bdid.Symbol);

            var bitsn = (BuiltinTypeSymbolNode)bdid.Symbol;

            Assert.AreEqual(FrameworkType.String, bitsn.BuiltinType);
        }

        [Test]
        public void FrameworkFuncBindingTest()
        {
            const string Code =
            @"
            Output(4);
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(1, tree!.Root.TopLevelStatements.Count);
            Assert.IsInstanceOf<FunctionCallStatementNode>(tree.Root.TopLevelStatements[0]);

            var fcsn = (FunctionCallStatementNode)tree.Root.TopLevelStatements[0];

            Assert.IsInstanceOf<IdentifierExpressionNode>(fcsn.FunctionExpression);

            var idex = (IdentifierExpressionNode)fcsn.FunctionExpression;

            Assert.IsInstanceOf<BoundIdentifierNode>(idex.IdentifierNode);

            var bdid = (BoundIdentifierNode)idex.IdentifierNode;

            Assert.IsInstanceOf<BuiltinFunctionSymbolNode>(bdid.Symbol);

            var func = (BuiltinFunctionSymbolNode)bdid.Symbol;

            Assert.AreEqual("Output", func.Name);
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

            SyntaxTree? tree = null;
            Assert.DoesNotThrow(() => tree = Analyser.Analyse(Code));

            Assert.NotNull(tree);

            var fcsn = (FunctionCallStatementNode)tree!.Root.TopLevelStatements[1];
            var fnex = (IdentifierExpressionNode)fcsn.FunctionExpression;
            var bdid = (BoundIdentifierNode)fnex.IdentifierNode;

            Assert.IsInstanceOf<LocalVariableSymbolNode>(bdid.Symbol);
        }
    }
}
