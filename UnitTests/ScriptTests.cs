using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class ScriptTests
    {
        [Test]
        public void WholeScriptWithoutBindingTest()
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

            Lexer lexer = new(Script);
            LexemeCollection lexemes = lexer.LexAll();

            ScriptParser parser = new(lexemes);
            SyntaxTree? tree = parser.ParseWholeScript();

            Assert.NotNull(tree);
            Assert.IsInstanceOf<ScriptNode>(tree!.Root);

            ScriptNode scriptNode = tree.Root;

            Assert.AreEqual(4, scriptNode.TopLevelStatements.Count);

            Assert.IsInstanceOf<FunctionCallStatementNode>(scriptNode.TopLevelStatements[0]);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(scriptNode.TopLevelStatements[1]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(scriptNode.TopLevelStatements[2]);
            Assert.IsInstanceOf<WhileStatementNode>(scriptNode.TopLevelStatements[3]);

            WhileStatementNode @while = (WhileStatementNode)scriptNode.TopLevelStatements[3];

            Assert.AreEqual(1, @while.Statements.Count);
        }
    }
}
