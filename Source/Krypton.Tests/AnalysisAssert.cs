using Krypton.Analysis;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Syntax;
using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Expressions;
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

        public static IEnumerable<Diagnostic> LexesWithError(string code, DiagnosticsCode errorCode)
        {
            var diagnostics = LexesWithDiagnostic(code).Where(d => d.DiagnosticCode == errorCode && d.IsError);
            Assert.IsTrue(diagnostics.Any());
            return diagnostics;
        }

        public static ExpressionNode ParsesExpressionSuccessfully(string code)
        {
            Analyser analyser = new(code);
            Lexer lexer = new(code, analyser);

            var tokens = lexer.LexAll();
            Assert.IsTrue(tokens.HasValue);

            ExpressionParser parser = new(tokens!.Value, analyser);

            int index = 0;

            var expression = parser.ParseNextExpression(ref index);
            Assert.IsNotNull(expression);

            return expression!;
        }

        public static ProgramNode ParsesSuccessfully(string code)
        {
            Analyser analyser = new(code);
            Lexer lexer = new(code, analyser);

            var tokens = lexer.LexAll();
            Assert.IsTrue(tokens.HasValue);

            ProgramParser parser = new(tokens!.Value, analyser);

            var program = parser.ParseWholeProgram();
            Assert.IsNotNull(program);

            Assert.AreEqual(0, analyser.Diagnostics.Count);

            return program!;
        }

        public static ReadOnlyCollection<Diagnostic> ParsesWithDiagnostic(string code)
        {
            Analyser analyser = new(code);
            Lexer lexer = new(code, analyser);

            var tokens = lexer.LexAll();
            Assert.IsTrue(tokens.HasValue);

            ProgramParser parser = new(tokens!.Value, analyser);

            var program = parser.ParseWholeProgram();
            Assert.IsNull(program);

            return analyser.Diagnostics;
        }

        public static IEnumerable<Diagnostic> ParsesWithError(string code, DiagnosticsCode errorCode)
        {
            var diagnostics = ParsesWithDiagnostic(code).Where(d => d.DiagnosticCode == errorCode && d.IsError);
            Assert.IsTrue(diagnostics.Any());
            return diagnostics;
        }
    }
}
