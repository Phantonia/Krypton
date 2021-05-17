using Krypton.Analysis;
using Krypton.Analysis.Lexical;
using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Krypton.Tests
{
    public static class AnalysisAssert
    {
        public static FinalList<Token> LexesSuccessfully(string code)
        {
            Analyser analyser = new(code);
            Lexer lexer = new(code, analyser);

            var tokens = lexer.LexAll();
            Assert.IsTrue(tokens.HasValue);

            return tokens!.Value;
        }

        public static ReadOnlyCollection<Diagnostic> LexesWithDiagnostic(string code)
        {
            Analyser analyser = new(code);
            Lexer lexer = new(code, analyser);

            var tokens = lexer.LexAll();
            Assert.IsFalse(tokens.HasValue);

            return analyser.Diagnostics;
        }

        public static IEnumerable<Diagnostic> LexesWithError(string code, DiagnosticsCode diagnosticsCode)
        {
            var diagnostics = LexesWithDiagnostic(code).Where(d => d.DiagnosticCode == diagnosticsCode && d.IsError);
            Assert.IsTrue(diagnostics.Any());
            return diagnostics;
        }
    }
}
