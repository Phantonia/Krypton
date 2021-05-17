using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class NumberTests
    {
        [TestMethod]
        public void TestIntegersBase10()
        {
            TestLiteral("1", 1);
            TestLiteral("0", 0);
            TestLiteral("256", 256);
            TestLiteral("6", 6);
        }

        [TestMethod]
        public void TestIntegersBase16()
        {
            TestLiteral("0x1", 0x1);
            TestLiteral("0x0", 0x0);
            TestLiteral("0xF", 0xF);
            TestLiteral("0xf", 0xF);
            TestLiteral("0xabc", 0xABC);
            TestLiteral("0xa1b2", 0xA1B2);
            TestLiteral("0x123fff", 0x123FFF);
        }

        [TestMethod]
        public void TestIntegersBase2()
        {
            TestLiteral("0b0", 0b0);
            TestLiteral("0b1", 0b1);
            TestLiteral("0b10101010", 0b10101010);
            TestLiteral("0b1111", 0b1111);
        }

        [TestMethod]
        public void TestHexValueWithMixedCase()
        {

        }

        private static void TestLiteral(string code, int value)
        {
            var tokens = AnalysisAssert.LexesSuccessfully(code);

            Assert.AreEqual(2, tokens.Count);

            var literal = tokens[0] as LiteralToken<long>;

            Assert.IsNotNull(literal);
            Assert.AreEqual(value, literal.Value);
        }
    }
}