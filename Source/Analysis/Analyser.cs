using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;
using Krypton.Analysis.Syntactical;
using Krypton.CompilationData;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.Analysis
{
    public sealed class Analyser
    {
        public Analyser(string code)
        {
            this.code = code;
        }

        private readonly string code;

        public event DiagnosticsEventHandler? DiagnosticReported;

        public Compilation? Analyse()
        {
            throw new NotImplementedException();
            // For unit tests
            //FrameworkIntegration.Reset();

            //Lexer lexer = new(code);
            //LexemeCollection? lexemes = lexer.LexAll();

            //if (lexemes == null)
            //{
            //    return null;
            //}

            //ProgramParser parser = new(lexemes, code);
            //ProgramNode? program = parser.ParseWholeProgram();

            //if (program == null)
            //{
            //    return null;
            //}

            //Compilation compilation = new(program, code);

            //Binder binder = new(compilation);
            //bool success = binder.PerformBinding();
            ////SemanticalAnalyser semanticalAnalyser = new(compilation);
            ////bool success = semanticalAnalyser.PerformSemanticalAnalysis();

            //if (!success)
            //{
            //    return null;
            //}

            //return compilation;
        }

        internal void ReportDiagnostic(Diagnostic diagnostic)
        {
            DiagnosticsEventArgs e = new(diagnostic);
            DiagnosticReported?.Invoke(this, e);
        }
    }
}
