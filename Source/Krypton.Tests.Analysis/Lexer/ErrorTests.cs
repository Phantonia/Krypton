using Krypton.Analysis;
using Krypton.CompilationData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Krypton.Tests.Analysis.Lexer
{
    [TestClass]
    public sealed class ErrorTests
    {
        private DiagnosticsMessageProvider messageProvider;

        [TestInitialize]
        public void Init()
        {
            messageProvider = new DiagnosticsMessageProvider();
        }

        [TestMethod]
        public void TestHexValueWithMixedCase()
        {
            const string Code = "0xaBcD";
            const DiagnosticsCode DiagnosticsCode = DiagnosticsCode.HexLiteralWithMixedCase;

            var diagnostics = AnalysisAssert.LexesWithDiagnostic(Code);

            Assert.AreEqual(1, diagnostics.Count);

            bool matches = diagnostics[0] is
            {
                DiagnosticCode: DiagnosticsCode,
                Message: string msg,
                IsError: true,
                IsWarning: false
            } && msg == messageProvider[DiagnosticsCode];
            Assert.IsTrue(matches);
        }

        [TestMethod]
        public void TestUnknownToken()
        {
            const string Code = "§";
            const DiagnosticsCode DiagnosticsCode = DiagnosticsCode.UnknownLexeme;

            var diagnostics = AnalysisAssert.LexesWithDiagnostic(Code);

            Assert.AreEqual(1, diagnostics.Count);

            bool matches = diagnostics[0] is
            {
                DiagnosticCode: DiagnosticsCode,
                Message: string msg,
                IsError: true,
                IsWarning: false
            } && msg == messageProvider[DiagnosticsCode];
            Assert.IsTrue(matches);
        }
    }
}