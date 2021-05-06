using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using System.Collections.Immutable;

namespace Krypton.CompilationData
{
    public sealed record Diagnostic(DiagnosticsCode DiagnosticCode,
                                    string Message,
                                    ImmutableArray<string> Details,
                                    string CodeLine,
                                    int LineIndex,
                                    bool IsError,
                                    Token OffendingToken,
                                    SyntaxNode? OffendingNode = null)
    {
        public bool IsWarning => !IsError;

        public int LineNumber => OffendingToken.LineNumber;
    }
}
