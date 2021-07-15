using Krypton.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Core
{
    [TestClass]
    public class RationalTests
    {
        [TestMethod]
        public void TestSimpleRationals()
        {
            TestRational(new Rational(2, 3), 2, 3);
            TestRational(new Rational(4, 6), 2, 3);
            TestRational(new Rational(200, 300), 2, 3);
            TestRational(new Rational(1, 2), 1, 2);
            TestRational(new Rational(1, 3), 1, 3);
            TestRational(new Rational(5, 2), 5, 2);
            TestRational(new Rational(10, 4), 5, 2);

            static void TestRational(Rational r, long numerator, long denominator)
            {
                Assert.AreEqual(r.Numerator, numerator);
                Assert.AreEqual(r.Denominator, denominator);
            }
        }

        [TestMethod]
        public void TestEqualsAndGetHashCode()
        {
            TestRational(new Rational(1, 2), new Rational(1, 2), expectedEquality: true);
            TestRational(new Rational(2, 3), new Rational(2, 3), expectedEquality: true);
            TestRational(new Rational(7, 4), new Rational(7, 4), expectedEquality: true);
            TestRational(new Rational(long.MaxValue - 100, short.MaxValue), new Rational(long.MaxValue - 100, short.MaxValue), expectedEquality: true);

            TestRational(new Rational(1, 2), new Rational(3, 6), expectedEquality: true);
            TestRational(new Rational(2, 4), new Rational(5, 10), expectedEquality: true);
            TestRational(new Rational(12345, 3 * 12345), new Rational(4 * 12345, 12 * 12345), expectedEquality: true);
            TestRational(new Rational(9, 4), new Rational(729, 324), expectedEquality: true);

            TestRational(new Rational(1, 2), new Rational(2, 3), expectedEquality: false);
            TestRational(new Rational(3, 2), new Rational(12, 5), expectedEquality: false);

            static void TestRational(Rational r1, Rational r2, bool expectedEquality)
            {
                Assert.AreEqual(expectedEquality, r1.Equals(r2));
                Assert.AreEqual(expectedEquality, r1 == r2);
                Assert.AreEqual(!expectedEquality, r1 != r2);
                
                // two unequal values may have the same hashcode
                if (expectedEquality)
                {
                    Assert.AreEqual(r1.GetHashCode(), r2.GetHashCode());
                }
            }
        }
    }
}
