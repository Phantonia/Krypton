using Krypton.Analysis;
using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class ScriptTests
    {
        [Test]
        public void WholeScriptTest()
        {
            const string Script =
            @"
            Output(""Hello world""); ... 0
            Var name = Input();      ... 1
            Output(""Hey"" + name);  ... 2
            While True               ... 3
            {
                Output(""Hey"");
            }
            ";

            SyntaxTree? tree = Analyser.Analyse(Script);

            Assert.NotNull(tree);
            Assert.IsInstanceOf<ScriptNode>(tree!.Root);

            ScriptNode scriptNode = tree.Root;

            Assert.AreEqual(4, scriptNode.Statements.Count);

            Assert.IsInstanceOf<FunctionCallStatementNode>(scriptNode.Statements[0]);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(scriptNode.Statements[1]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(scriptNode.Statements[2]);
            Assert.IsInstanceOf<WhileStatementNode>(scriptNode.Statements[3]);

            WhileStatementNode @while = (WhileStatementNode)scriptNode.Statements[3];

            Assert.AreEqual(1, @while.Statements.Count);
        }
    }
}
