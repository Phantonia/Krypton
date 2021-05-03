using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.CompilationData
{
    public sealed record Diagnostic(DiagnosticsCode DiagnosticCode,
                                    bool IsError,
                                    Token OffendingToken,
                                    SyntaxNode? OffendingNode = null)
    {
        public bool IsWarning => !IsError;

        public int LineNumber => OffendingToken.LineNumber;

        public string GetCode()
        {
            IWritable writable = (IWritable?)OffendingNode ?? OffendingToken;
            StringWriter output = new();
            writable.WriteCode(output);
            return output.ToString();
        }
    }
}
