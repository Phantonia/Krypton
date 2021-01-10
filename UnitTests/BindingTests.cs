using Krypton.Analysis;
using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Semantical.Binding;
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
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree!.Root.Statements[0]);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(tree.Root.Statements[1]);

            var decl = (VariableDeclarationStatementNode)tree!.Root.Statements[0];
            var assg = (VariableAssignmentStatementNode)tree!.Root.Statements[1];

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

            Assert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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

            var decl1 = tree!.Root.Statements[0] as VariableDeclarationStatementNode;
            var decl2 = tree!.Root.Statements[1] as VariableDeclarationStatementNode;

            Assert.NotNull(decl1);
            Assert.NotNull(decl2);

            var varX = decl1!.VariableIdentifierNode as BoundIdentifierNode;
            var varY = decl2!.VariableIdentifierNode as BoundIdentifierNode;

            Assert.NotNull(varX);
            Assert.NotNull(varY);

            var idX = decl2.Value as IdentifierExpressionNode;

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

            Assert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
        }

        [Test]
        public void BuiltinFunctionBindingTest()
        {
            const string Code =
            @"
            Output();
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.IsInstanceOf<FunctionCallStatementNode>(tree!.Root.Statements[0]);

            var call = (FunctionCallStatementNode)tree.Root.Statements[0];

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);

            var idex = (IdentifierExpressionNode)call.FunctionExpression;

            Assert.IsInstanceOf<BoundIdentifierNode>(idex.IdentifierNode);

            var bdid = (BoundIdentifierNode)idex.IdentifierNode;

            var actualOutputFunc = new BuiltinIdentifierMap()["Output"];
            var boundOutputFunc = bdid.Symbol;

            Assert.True(ReferenceEquals(actualOutputFunc, boundOutputFunc));
        }
    }
}
