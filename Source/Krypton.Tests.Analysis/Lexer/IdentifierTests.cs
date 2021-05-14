using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

                Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken));
                Assert.IsInstanceOfType(tokens[1], typeof(EndOfFileToken));

                Assert.AreEqual(code, tokens[0].TextToString());
            }
        }

        [TestMethod]
        public void TestKeywords()
        {
            TestToken<ReservedKeywordToken>("As");
            TestToken<ReservedKeywordToken>("Block");
            TestToken<ReservedKeywordToken>("Const");
            TestToken<ReservedKeywordToken>("Continue");
            TestToken<ReservedKeywordToken>("Else");
            TestToken<ReservedKeywordToken>("For");
            TestToken<ReservedKeywordToken>("Func");
            TestToken<ReservedKeywordToken>("If");
            TestToken<ReservedKeywordToken>("Leave");
            TestToken<ReservedKeywordToken>("Let");
            TestToken<ReservedKeywordToken>("Return");
            TestToken<ReservedKeywordToken>("To");
            TestToken<ReservedKeywordToken>("Var");
            TestToken<ReservedKeywordToken>("While");
            TestToken<ReservedKeywordToken>("With");

            var trueLiteral = TestToken<LiteralToken<bool>>("True");
            var falseLiteral = TestToken<LiteralToken<bool>>("False");

            Assert.AreEqual(true, trueLiteral.Value);
            Assert.AreEqual(false, falseLiteral.Value);

            TestToken<OperatorToken>("And");
            TestToken<OperatorToken>("Div");
            TestToken<OperatorToken>("Mod");
            TestToken<OperatorToken>("Not");
            TestToken<OperatorToken>("Or");
            TestToken<OperatorToken>("Xor");

            TestToken<IdentifierToken>("var");
            TestToken<IdentifierToken>("true");
            TestToken<IdentifierToken>("xor");

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