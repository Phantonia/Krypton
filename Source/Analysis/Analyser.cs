using Krypton.Analysis.AST;
using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;

namespace Krypton.Analysis
{
    public static class Analyser
    {
        public static SyntaxTree? Analyse(string code)
        {
            Lexer lexer = new(code);
            LexemeCollection lexemes = lexer.LexAll();

            ProgramParser parser = new(lexemes);
            SyntaxTree? syntaxTree = parser.ParseWholeProgram();

            if (syntaxTree == null)
            {
                return null;
            }

            SemanticalAnalyser semanticalAnalyser = new(syntaxTree);
            bool success = semanticalAnalyser.PerformSemanticalAnalysis();

            if (!success)
            {
                return null;
            }

            return syntaxTree;
        }
    }
}
