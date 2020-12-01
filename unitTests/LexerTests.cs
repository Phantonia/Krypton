using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class LexerTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void NumberTests()
        {
            LexemeCollection lexemes;

            // Integer literal base 10
            lexemes = new Lexer("1 10 100 1_ 1_0 10_ 100_000 1_000_000 1__0").LexAll();
            List<IntegerLiteralLexeme> lexemes2 = null!;
            Assert.DoesNotThrow(() =>
            {
                lexemes2 = lexemes.Take(lexemes.Count - 1)
                                  .Cast<IntegerLiteralLexeme>()
                                  .ToList();
            }, "At least one of the lexemes is not an integer literal");

            long[] expected = new[] { 1L, 10L, 100L, 1L, 10L, 10L, 100_000L, 1_000_000L, 10L };

            Assert.AreEqual(expected.Length, lexemes2.Count);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], lexemes2[i].Value, $"Expected value {expected[i]}; got value {lexemes2[i].Value}");
            }
        }
    }
}