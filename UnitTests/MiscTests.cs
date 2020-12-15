using Krypton.Analysis.Lexical;
using NUnit.Framework;

namespace UnitTests
{
    public class MiscTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void RationalEqualityTest()
        {
            RationalLiteralValue r1 = new(314, 2);
            RationalLiteralValue r2 = new(3140, 3);

            Assert.IsTrue(r1 == r2);
            Assert.AreEqual(r1, r2);
            Assert.AreEqual(r1.GetHashCode(), r2.GetHashCode());
        }

        [Test]
        public void RationalParserAndEqualityTest()
        {
            RationalLiteralValue r1 = NumberLiteralParser.ParseRational("2.718");
            RationalLiteralValue r2 = NumberLiteralParser.ParseRational("2.7180000");

            Assert.IsTrue(r1 == r2);
            Assert.IsTrue(r1.Numerator == r2.Numerator);
        }
    }
}
