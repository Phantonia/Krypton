using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    public sealed class SyntaxTreeTests
    {
        [Test]
        public void EnumeratingTest()
        {
            Node root = SampleNode();

            SyntaxTree tree = new(root);

            IEnumerator<Node> enumerator = tree.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(enumerator.Current);

            VariableDeclarationStatementNode varDecl = (VariableDeclarationStatementNode)enumerator.Current;

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(varDecl.IdentifierNode, enumerator.Current);
            Assert.AreEqual(varDecl.Identifier, ((IdentifierNode)enumerator.Current).Identifier);
        }

        [Test]
        public void CloningTest()
        {
            Node root = SampleNode();
            Node clone = root.Clone();

            Assert.IsFalse(ReferenceEquals(root, clone));

            var varDeclA = (VariableDeclarationStatementNode)root;
            var varDeclB = (VariableDeclarationStatementNode)clone;

            Assert.IsFalse(ReferenceEquals(varDeclA.Value, varDeclB.Value));
        }

        [Test]
        public void PreviousAndNextForStatementsTest()
        {
            FunctionCallStatementNode f1 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Output", 1), 1), 1);
            FunctionCallStatementNode f2 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Input", 1), 1), 1);
            FunctionCallStatementNode f3 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Sin", 1), 1), 1);

            BlockStatementNode statement = new BlockStatementNode(new[] { f1, f2, f3 }, 1);

            Assert.IsTrue(ReferenceEquals(statement[1], statement[0].Next));
            Assert.IsTrue(ReferenceEquals(statement[0], statement[1].Previous));
            Assert.IsTrue(ReferenceEquals(statement, f1.Parent));
        }

        private static Node SampleNode() => new VariableDeclarationStatementNode(new IdentifierNode("x", 1),
                                                                                 new IdentifierTypeNode("Int", 1),
                                                                                 new FunctionCallExpressionNode(new IdentifierExpressionNode("Y", 1),
                                                                                                                new[] {
                                                                                                                           new BitwiseOrBinaryOperationExpressionNode(new IntegerLiteralExpressionNode(256, 1),
                                                                                                                                                                      new IntegerLiteralExpressionNode(69, 1),
                                                                                                                                                                      1)
                                                                                                                      }, 1), 1);
    }
}
