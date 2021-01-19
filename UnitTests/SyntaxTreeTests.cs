using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.BinaryOperations;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.TypeSpecs;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public sealed class SyntaxTreeTests
    {
        [Test]
        public void EnumeratingTest()
        {
            StatementNode node = (StatementNode)SampleNode();
            ProgramNode root = new(new StatementCollectionNode(Enumerable.Repeat(node, 1)), 1);

            SyntaxTree tree = new(root);

            IEnumerator<Node> enumerator = tree.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsInstanceOf<ProgramNode>(enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsInstanceOf<StatementCollectionNode>(enumerator.Current);

            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(enumerator.Current);

            VariableDeclarationStatementNode varDecl = (VariableDeclarationStatementNode)enumerator.Current;

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(varDecl.VariableIdentifierNode, enumerator.Current);
            Assert.AreEqual(varDecl.Identifier, ((IdentifierNode)enumerator.Current).Identifier);
        }

        [Test]
        public void PreviousAndNextForStatementsTest()
        {
            FunctionCallStatementNode f1 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Output", 1), 1), 1);
            FunctionCallStatementNode f2 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Input", 1), 1), 1);
            FunctionCallStatementNode f3 = new(new FunctionCallExpressionNode(new IdentifierExpressionNode("Sin", 1), 1), 1);

            StatementCollectionNode statement = new(new[] { f1, f2, f3 });

            Assert.IsTrue(ReferenceEquals(statement[1], statement[0].Next));
            Assert.IsTrue(ReferenceEquals(statement[0], statement[1].Previous));
            Assert.IsTrue(ReferenceEquals(statement, f1.Parent));
        }

        private static Node SampleNode() => new VariableDeclarationStatementNode(new UnboundIdentifierNode("x", 1),
                                                                                 new IdentifierTypeSpecNode("Int", 1),
                                                                                 new FunctionCallExpressionNode(new IdentifierExpressionNode("Y", 1),
                                                                                                                new[] {
                                                                                                                           new BitwiseOrBinaryOperationExpressionNode(new IntegerLiteralExpressionNode(256, 1),
                                                                                                                                                                      new IntegerLiteralExpressionNode(69, 1),
                                                                                                                                                                      1)
                                                                                                                      }, 1), 1);
    }
}
