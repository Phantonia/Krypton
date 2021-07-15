using Krypton.CompilationData.Syntax.Expressions;
using Krypton.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Parser
{
    [TestClass]
    public sealed class ExpressionTests
    {
        [TestMethod]
        public void TestLiteralExpression()
        {
            Test("4", 4L);
            Test("2.71", new Rational(271, 100));
            Test("007", 7L);
            Test("8i", new RationalComplex(0, 8));
            Test("1.61i", new RationalComplex(0, new Rational(161, 100)));

            static void Test<T>(string code, T expectedValue)
            {
                var expression = AnalysisAssert.ParsesExpressionSuccessfully(code);
                var literal = expression as LiteralExpressionNode<T>;

                Assert.IsNotNull(literal);

                Assert.AreEqual(expectedValue, literal.Value);
            }
        }
    }
}