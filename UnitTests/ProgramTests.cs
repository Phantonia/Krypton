using Krypton.Analysis;
using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Syntactical;
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

            ProgramParser parser = new(lexemes, Code);
            Compilation tree = new(parser.ParseWholeProgram()!, Code);

            Assert.NotNull(tree);
            Assert.IsInstanceOf<ProgramNode>(tree.Program);

            ProgramNode programNode = tree.Program;

            Assert.AreEqual(4, programNode.TopLevelStatementNodes.Count);

            Assert.IsInstanceOf<FunctionCallStatementNode>(programNode.TopLevelStatementNodes[0]);
            Assert.IsInstanceOf<VariableDeclarationStatementNode>(programNode.TopLevelStatementNodes[1]);
            Assert.IsInstanceOf<FunctionCallStatementNode>(programNode.TopLevelStatementNodes[2]);
            Assert.IsInstanceOf<WhileStatementNode>(programNode.TopLevelStatementNodes[3]);

            WhileStatementNode @while = (WhileStatementNode)programNode.TopLevelStatementNodes[3];

            Assert.AreEqual(1, @while.StatementNodes.Count);
        }
    }
}
