using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class TriviaTests
    {
        [TestMethod]
        public void TestWhitespace()
        {
            const string TwoSpaces = "  ";
            const string Tab = "\t";
            const string NewLine = "\r\n";
            const string Misc = " \t  \r\n  \t";

            TestForToken("*", TwoSpaces);
            TestForToken("/", TwoSpaces);
            TestForToken("->", Tab);
            TestForToken("And", Tab);
            TestForToken("Var", NewLine);
            TestForToken("id", NewLine);
            TestForToken(";", Misc);
            TestForToken(".", Misc);
            TestForToken("|", string.Empty);

            static void TestForToken(string token, string whitespace)
            {
                string code = whitespace + token; // like "  *"

                var tokens = AnalysisAssert.LexesSuccessfully(code);

                Assert.AreEqual(2, tokens.Count);
                Assert.AreEqual(token, tokens[0].TextToString());
                Assert.AreEqual(whitespace, tokens[0].LeadingTrivia.TextToString());
            }
        }
    }
}