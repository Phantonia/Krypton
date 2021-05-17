using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class StringTests
    {
        [TestMethod]
        public void TestSimpleString()
        {
            const string Code = @" ""abc"" "; //: "abc"
            TestLiteral(Code, "abc");
        }

        [TestMethod]
        public void TestEmptyString()
        {
            const string Code = @" """" "; //: ""
            TestLiteral(Code, "");
        }

        [TestMethod]
        public void TestEscapeSequences()
        {
            const string String = "a\t\0\u0018\"";
            const string Code = @" ""a\t\0\u0018\"""" "; //: "a\t\0\u0018\""
            TestLiteral(Code, String);
        }

        [TestMethod]
        public void TestUnclosedStringLiteral()
        {
            const string Code = @" ""str "; //: "str
            AnalysisAssert.LexesWithError(Code, DiagnosticsCode.UnclosedStringLiteral);
        }

        private static void TestLiteral(string code, string str)
        {
            var tokens = AnalysisAssert.LexesSuccessfully(code);

            Assert.AreEqual(2, tokens.Count);

            string value = tokens[0].AssertIsType<LiteralToken<string>>(t => t.Value) as string;
            Assert.IsNotNull(value);

            Assert.AreEqual(str, value);
        }
    }
}