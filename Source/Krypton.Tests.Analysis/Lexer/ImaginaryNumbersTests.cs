using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class ImaginaryNumbersTests
    {
        [TestMethod]
        public void TestImaginaryIntegers()
        {
            Test("4i", 4L);
            Test("69i", 69L); // nice
            Test("1729i", 1729L);
            Test(long.MaxValue + "i", long.MaxValue);

            static void Test(string code, long value)
            {
                var tokens = AnalysisAssert.LexesSuccessfully(code);

                Assert.AreEqual(2, tokens.Count);

                var literal = tokens[0] as LiteralToken<RationalComplex>;

                Assert.IsNotNull(literal);
                Assert.AreEqual(value, literal.Value.Imaginary);
            }
        }

        [TestMethod]
        public void TestImaginaryRationals()
        {
            Test("9.81i", new Rational(981, 100));
            Test("0.001i", new Rational(1, 1000));
            Test("10.0i", 10);

            static void Test(string code, Rational value)
            {
                var tokens = AnalysisAssert.LexesSuccessfully(code);

                Assert.AreEqual(2, tokens.Count);

                var literal = tokens[0] as LiteralToken<RationalComplex>;

                Assert.IsNotNull(literal);
                Assert.AreEqual(value, literal.Value.Imaginary);
            }
        }
    }
}
