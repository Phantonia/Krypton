using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class ProgramTests
    {
        [Test]
        public void WholeProgramWithoutBindingTest()
        {
            const string Code =
            @"
            Output(""Hello world""); ... 0
            Var name = Input();      ... 1
            Output(""Hey"" + name);  ... 2
            While True               ... 3
            {
                Output(""Hey"");
            }
            ";

            Lexer lexer = new(Code);
            LexemeCollection lexemes = lexer.LexAll();

            ProgramParser parser = new(lexemes);
            SyntaxTree? tree = parser.ParseWholeProgram();

            Assert.NotNull(tree);
            Assert.IsInstanceOf<ProgramNode>(tree!.Root);

            ProgramNode programNode = tree.Root;

            Assert.AreEqual(4, programNode.TopLevelStatements.Count);

            Assert.IsInstanceOf<FunctionCallStatementNode>(programNode.TopLevelStatements[0]);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(programNode.TopLevelStatements[1]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(programNode.TopLevelStatements[2]);
            Assert.IsInstanceOf<WhileStatementNode>(programNode.TopLevelStatements[3]);

            WhileStatementNode @while = (WhileStatementNode)programNode.TopLevelStatements[3];

            Assert.AreEqual(1, @while.Statements.Count);
        }
    }
}
