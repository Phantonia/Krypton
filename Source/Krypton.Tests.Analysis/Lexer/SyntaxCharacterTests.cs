using Krypton.CompilationData.Syntax.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Sc = Krypton.CompilationData.Syntax.Tokens.SyntaxCharacterToken;
using Op = Krypton.CompilationData.Syntax.Tokens.OperatorToken;

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
                typeof(Sc), // ;
                typeof(Sc), // ,
                typeof(Sc), // :
                typeof(Sc), // .
                typeof(Sc), // (
                typeof(Sc), // )
                typeof(Sc), // [
                typeof(Sc), // ]
                typeof(Sc), // {
                typeof(Sc), // }
                typeof(Op), // <
                typeof(Op), // >
                typeof(Sc), // =
                typeof(Op), // +
                typeof(Op), // -
                typeof(Op), // *
                typeof(Op), // /
                typeof(Op), // &
                typeof(Op), // |
                typeof(Op), // ^
                typeof(Op), // ~
                typeof(Op), // ==
                typeof(Op), // **
                typeof(Op), // !=
                typeof(Op), // <=
                typeof(Op), // >=
                typeof(Op), // ->
                typeof(Op), // <-
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