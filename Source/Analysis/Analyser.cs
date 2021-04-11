using Krypton.Analysis.Syntactical;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantical;
using System.Runtime.CompilerServices;
using Krypton.CompilationData.Syntax;
using System.IO;
using Krypton.CompilationData;
using System.Collections.Generic;
using System.Diagnostics;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.Analysis
{
    public sealed class Analyser
    {
        public Analyser(TextReader input)
        {
            this.input = input;
        }

        private readonly List<Diagnostic> diagnostics = new();
        private readonly TextReader input;

        public event DiagnosticEventHandler? Error;

        public event DiagnosticEventHandler? Warning;

        public Compilation Analyse()
        {
        }

        internal void ReportDiagnostic(Diagnostic diagnostic)
        {
            diagnostics.Add(diagnostic);

            if (diagnostic.IsError)
            {
                Error?.Invoke(this, new DiagnosticEventArgs(diagnostic));
            }
            else
            {
                Debug.Assert(diagnostic.IsWarning);
                Warning?.Invoke(this, new DiagnosticEventArgs(diagnostic));
            }
        }
    }
}
