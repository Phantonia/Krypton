using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rk = Krypton.CompilationData.Syntax.Tokens.ReservedKeywordToken;
using Op = Krypton.CompilationData.Syntax.Tokens.OperatorToken;
using Id = Krypton.CompilationData.Syntax.Tokens.IdentifierToken;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class IdentifierAndKeywordTests
    {
        [TestMethod]
        public void TestDifferentIdentifiers()
        {
            TestIdentifier("Identifier");
            TestIdentifier("n");
            TestIdentifier("n_2");
            TestIdentifier("_id");
            TestIdentifier("_0");
            TestIdentifier("abc_123");
            TestIdentifier("a_b");
            TestIdentifier("__");

            static void TestIdentifier(string code)
            {
                var tokens = AnalysisAssert.LexesSuccessfully(code);

                Assert.AreEqual(2, tokens.Count);

                Assert.IsInstanceOfType(tokens[0], typeof(Id));
                Assert.IsInstanceOfType(tokens[1], typeof(EndOfFileToken));

                Assert.AreEqual(code, tokens[0].TextToString());
            }
        }

        [TestMethod]
        public void TestKeywords()
        {
            TestToken<Rk>("As");
            TestToken<Rk>("Block");
            TestToken<Rk>("Const");
            TestToken<Rk>("Continue");
            TestToken<Rk>("Else");
            TestToken<Rk>("For");
            TestToken<Rk>("Func");
            TestToken<Rk>("If");
            TestToken<Rk>("Leave");
            TestToken<Rk>("Let");
            TestToken<Rk>("Return");
            TestToken<Rk>("To");
            TestToken<Rk>("Var");
            TestToken<Rk>("While");
            TestToken<Rk>("With");

            var trueLiteral = TestToken<LiteralToken<bool>>("True");
            var falseLiteral = TestToken<LiteralToken<bool>>("False");

            Assert.AreEqual(true, trueLiteral.Value);
            Assert.AreEqual(false, falseLiteral.Value);

            TestToken<Op>("And");
            TestToken<Op>("Div");
            TestToken<Op>("Mod");
            TestToken<Op>("Not");
            TestToken<Op>("Or");
            TestToken<Op>("Xor");

            TestToken<Id>("var");
            TestToken<Id>("true");
            TestToken<Id>("xor");

            static T TestToken<T>(string code)
                where T : Token
            {
                var tokens = AnalysisAssert.LexesSuccessfully(code);

                Assert.AreEqual(2, tokens.Count);

                Assert.IsInstanceOfType(tokens[0], typeof(T));
                Assert.IsInstanceOfType(tokens[1], typeof(EndOfFileToken));

                Assert.AreEqual(code, tokens[0].TextToString());

                return tokens[0] as T;
            }
        }
    }
}