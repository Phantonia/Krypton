using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class LexerTests
    {
        private static LexemeCollection LexCode(string code)
        {
            return new Lexer(code).LexAll();
        }

        [Test]
        public void SimpleStatementTest()
        {
            LexemeCollection lexemes = LexCode("Var number As Int = 4 + Sin(PI)");

            Assert.IsTrue(lexemes[0] is KeywordLexeme { Keyword: ReservedKeyword.Var });
            Assert.IsTrue(lexemes[1] is IdentifierLexeme { Content: "number" });
            Assert.IsTrue(lexemes[2] is KeywordLexeme { Keyword: ReservedKeyword.As });
            Assert.IsTrue(lexemes[3] is IdentifierLexeme { Content: "Int" });
            Assert.IsTrue(lexemes[4] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.Equals });
            Assert.IsTrue(lexemes[5] is IntegerLiteralLexeme { Value: 4 });
            Assert.IsTrue(lexemes[6] is CharacterOperatorLexeme { Operator: CharacterOperator.Plus });
            Assert.IsTrue(lexemes[7] is IdentifierLexeme { Content: "Sin" });
            Assert.IsTrue(lexemes[8] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisOpening });
            Assert.IsTrue(lexemes[9] is IdentifierLexeme { Content: "PI" });
            Assert.IsTrue(lexemes[10] is SyntaxCharacterLexeme { SyntaxCharacter: SyntaxCharacter.ParenthesisClosing });
            Assert.IsTrue(lexemes[11] is EndOfFileLexeme);
        }

        [Test]
        public void NumberTests()
        {
            // Integer literal base 10
            LexemeCollection lexemes = LexCode("1 10 100 1_ 1_0 10_ 100_000 1_000_000 1__0");

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
            LexemeCollection lexemes = new Lexer("4 & 7 -> 9 | 2 <- 11 ^ 1").LexAll();

            Assert.AreEqual(12, lexemes.Count);

            Assert.IsTrue(lexemes[1] is CharacterOperatorLexeme { Operator: CharacterOperator.Ampersand });
            Assert.IsTrue(lexemes[3] is CharacterOperatorLexeme { Operator: CharacterOperator.RightShift });
            Assert.IsTrue(lexemes[5] is CharacterOperatorLexeme { Operator: CharacterOperator.Pipe });
            Assert.IsTrue(lexemes[7] is CharacterOperatorLexeme { Operator: CharacterOperator.LeftShift });
            Assert.IsTrue(lexemes[9] is CharacterOperatorLexeme { Operator: CharacterOperator.Caret });
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

        [Test]
        public void NotEqualsTest()
        {
            LexemeCollection lexemes = LexCode("a != b");

            Assert.IsTrue(lexemes[1] is CharacterOperatorLexeme { Operator: CharacterOperator.ExclamationEquals });
        }
    }
}