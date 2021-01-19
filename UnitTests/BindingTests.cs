using Krypton.Analysis;
using Krypton.Analysis.AST;
using Krypton.Analysis.AST.Expressions;
using Krypton.Analysis.AST.Identifiers;
using Krypton.Analysis.AST.Statements;
using Krypton.Analysis.AST.Symbols;
using Krypton.Analysis.AST.TypeSpecs;
using Krypton.Analysis.Semantical.Binding;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

            MyAssert.Throws<NotImplementedException>(() => Analyser.Analyse(Code));
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
            Assert.IsInstanceOf<FunctionCallStatementNode>(tree!.Root.TopLevelStatements[0]);

            var call = (FunctionCallStatementNode)tree.Root.TopLevelStatements[0];

            Assert.IsInstanceOf<IdentifierExpressionNode>(call.FunctionExpression);

            var idex = (IdentifierExpressionNode)call.FunctionExpression;

            Assert.IsInstanceOf<BoundIdentifierNode>(idex.IdentifierNode);

            var bdid = (BoundIdentifierNode)idex.IdentifierNode;

            var actualOutputFunc = new BuiltinIdentifierMap()["Output"];
            var boundOutputFunc = bdid.Symbol;

            Assert.True(ReferenceEquals(actualOutputFunc, boundOutputFunc));
        }

        [Test]
        public void TypeBindingTest()
        {
            const string Code =
            @"
            Var str As String;
            str = ""x"";
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(2, tree!.Root.TopLevelStatements.Count);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(tree.Root.TopLevelStatements[0]);
            Assert.IsInstanceOf<VariableAssignmentStatementNode>(tree.Root.TopLevelStatements[1]);

            var decl = (VariableDeclarationStatementNode)tree.Root.TopLevelStatements[0];

            Assert.NotNull(decl.Type);
            Assert.IsInstanceOf<IdentifierTypeSpecNode>(decl.Type);

            var idtp = (IdentifierTypeSpecNode)decl.Type!;

            Assert.IsInstanceOf<BoundIdentifierNode>(idtp.IdentifierNode);

            var bound = (BoundIdentifierNode)idtp.IdentifierNode;

            var actStringType = new BuiltinIdentifierMap()["String"];
            var bndStringType = bound.Symbol;

            Assert.True(ReferenceEquals(actStringType, bndStringType));
        }

        [Test]
        public void MoreTypeBindingTest()
        {
            const string Code =
            @"
            Var x As Int;
            Var y As Rational;
            Var z As Complex;
            Var a As Bool;
            ";

            SyntaxTree? tree = Analyser.Analyse(Code);

            Assert.NotNull(tree);
            Assert.AreEqual(4, tree!.Root.TopLevelStatements.Count);

            BuiltinIdentifierMap bim = new();
            SymbolNode?[] symbols =
            {
                bim["Int"],
                bim["Rational"],
                bim["Complex"],
                bim["Bool"]
            };

            int i = 0;

            foreach (var statement in tree.Root.TopLevelStatements)
            {
                Assert.IsInstanceOf<VariableDeclarationStatementNode>(statement);

                TypeSpecNode? typeSpec = ((VariableDeclarationStatementNode)statement).Type;

                Assert.NotNull(typeSpec);
                Assert.IsInstanceOf<IdentifierTypeSpecNode>(typeSpec);

                var id = (IdentifierTypeSpecNode)typeSpec!;

                Assert.IsInstanceOf<BoundIdentifierNode>(id.IdentifierNode);

                var bound = (BoundIdentifierNode)id.IdentifierNode;

                Assert.IsInstanceOf<TypeSymbolNode>(bound.Symbol);
                Assert.True(ReferenceEquals(symbols[i], (TypeSymbolNode)bound.Symbol));

                i++;
            }
        }
    }
}
