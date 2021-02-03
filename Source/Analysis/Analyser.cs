using Krypton.Analysis.Ast;
using Krypton.Analysis.Syntactical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.Analysis
{
    public static class Analyser
    {
        public static Compilation? Analyse(string code)
        {
            Lexer lexer = new(code);
            LexemeCollection lexemes = lexer.LexAll();

            ProgramParser parser = new(lexemes);
            ProgramNode? program = parser.ParseWholeProgram();

            if (program == null)
            {
                return null;
            }

            Compilation compilation = new(program);

            SemanticalAnalyser semanticalAnalyser = new(compilation);
            bool success = semanticalAnalyser.PerformSemanticalAnalysis();

            if (!success)
            {
                return null;
            }

            return compilation;
        }
    }
}
