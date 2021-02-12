using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Syntactical;
using NUnit.Framework;

namespace UnitTests
{
    class FunctionTests
    {
        [Test]
        public void FunctionDeclarationNoParametersNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld()
            {
                Output(""Hello world"");
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(0, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationOneParameterNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As String)
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(1, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationTwoParametersNoReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As String, y As Int)
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(2, func.ParameterNodes.Count);
            Assert.IsNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationNoParametersWithReturnTest()
        {
            const string Code = @"
            Func HelloWorld() As Int
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(0, func.ParameterNodes.Count);
            Assert.NotNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }

        [Test]
        public void FunctionDeclarationTwoParametersWithReturnTest()
        {
            const string Code = @"
            Func HelloWorld(x As Complex, y As Char) As Int
            {
                Output(""Hello "" + x);
            }
            ";

            FunctionDeclarationNode? func = MyAssert.NoError(() =>
            {
                var lexemes = new Lexer(Code).LexAll();
                var parser = new ProgramParser(lexemes, Code);
                return parser.ParseFunctionDeclaration();
            });

            Assert.NotNull(func);
            Assert.AreEqual("HelloWorld", func!.Identifier);
            Assert.AreEqual(2, func.ParameterNodes.Count);
            Assert.NotNull(func.ReturnTypeNode);
            Assert.AreEqual(1, func.BodyNode.Count);
        }
    }
}
