using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class SyntaxCharacterTests
    {
        [TestMethod]
        public void TestSyntaxCharacters()
        {
            const string Code = "; , : . ( ) [ ] { } < > = + - * / & | ^ ~ == ** != <= >= -> <-";

            Type[] expectedTypes =
            {
                typeof(SyntaxCharacterToken), // ;
                typeof(SyntaxCharacterToken), // ,
                typeof(SyntaxCharacterToken), // :
                typeof(SyntaxCharacterToken), // .
                typeof(SyntaxCharacterToken), // (
                typeof(SyntaxCharacterToken), // )
                typeof(SyntaxCharacterToken), // [
                typeof(SyntaxCharacterToken), // ]
                typeof(SyntaxCharacterToken), // {
                typeof(SyntaxCharacterToken), // }
                typeof(OperatorToken), // <
                typeof(OperatorToken), // >
                typeof(SyntaxCharacterToken), // =
                typeof(OperatorToken), // +
                typeof(OperatorToken), // -
                typeof(OperatorToken), // *
                typeof(OperatorToken), // /
                typeof(OperatorToken), // &
                typeof(OperatorToken), // |
                typeof(OperatorToken), // ^
                typeof(OperatorToken), // ~
                typeof(OperatorToken), // ==
                typeof(OperatorToken), // **
                typeof(OperatorToken), // !=
                typeof(OperatorToken), // <=
                typeof(OperatorToken), // >=
                typeof(OperatorToken), // ->
                typeof(OperatorToken), // <-
                typeof(EndOfFileToken),
            };

            string[] expectedTexts = { ";", ",", ":", ".", "(", ")", "[", "]",
                                       "{", "}", "<", ">", "=", "+", "-", "*",
                                       "/", "&", "|", "^", "~", "==", "**", "!=",
                                       "<=", ">=", "->", "<-", string.Empty };

            var tokens = AnalysisAssert.LexesSuccessfully(Code);

            Assert.AreEqual(expectedTypes.Length, tokens.Count);
            Assert.AreEqual(expectedTexts.Length, tokens.Count);

            foreach (var (token, type) in tokens.Zip(expectedTypes))
            {
                Assert.IsInstanceOfType(token, type);
            }

            foreach (var (token, text) in tokens.Zip(expectedTexts))
            {
                Assert.AreEqual(text, new string(token.Text.Span));
            }
        }
    }
}