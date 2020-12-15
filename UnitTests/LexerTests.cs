using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using NUnit.Framework;
using System;
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

        [Test]
        public void NewOperatorsTest()
        {
            LexemeCollection lexemes = new Lexer("4 & 7 Right 9 | 2 Left 11 ^ 1").LexAll();

            Assert.AreEqual(12, lexemes.Count);

            Assert.IsAssignableFrom<AmpersandLexeme>(lexemes[1]);
            Assert.IsAssignableFrom<RightKeywordLexeme>(lexemes[3]);
            Assert.IsAssignableFrom<PipeLexeme>(lexemes[5]);
            Assert.IsAssignableFrom<LeftKeywordLexeme>(lexemes[7]);
            Assert.IsAssignableFrom<CaretLexeme>(lexemes[9]);
        }

        [Test]
        public void RationalLiteralTest()
        {
            bool success = NumberLiteralParser.TryParseRational("3.14", out RationalLiteralValue value);

            Assert.IsTrue(success);
            Assert.AreEqual(314, value.Numerator);
            Assert.AreEqual(2, value.Power);
            Assert.AreEqual(3.14, (double)value);
            Assert.AreEqual(314 * Math.Pow(10, -2), (double)value);

            success = NumberLiteralParser.TryParseRational("0.0001", out value);
            Assert.IsTrue(success);
            Assert.AreEqual(1, value.Numerator);
            Assert.AreEqual(4, value.Power);
            Assert.AreEqual(0.0001, (double)value);
        }

        [Test]
        public void RationalLiteralLexerTest()
        {
            Lexer lexer = new("3.14159");
            LexemeCollection lexemes = lexer.LexAll();

            Assert.AreEqual(2, lexemes.Count);

            Assert.IsAssignableFrom<RationalLiteralLexeme>(lexemes[0]);

            RationalLiteralLexeme r = (RationalLiteralLexeme)lexemes[0];

            Assert.AreEqual(3.14159, (double)r.Value);
        }
    }
}