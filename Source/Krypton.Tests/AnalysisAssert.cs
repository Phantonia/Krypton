using Krypton.Analysis.Lexical;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests
{
    public static class AnalysisAssert
    {
        public static FinalList<Token> LexesSuccessfully(string code)
        {
            //Analyser analyser = new(code);
            Lexer lexer = new(code);//, analyser);

            var tokens = lexer.LexAll();
            Assert.IsTrue(tokens.HasValue);

            return tokens!.Value;
        }
    }
}
