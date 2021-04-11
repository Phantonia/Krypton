using Krypton.CompilationData;
using System;

namespace Krypton.Analysis
{
    public delegate void DiagnosticEventHandler(Analyser sender, DiagnosticEventArgs e);

    public sealed class DiagnosticEventArgs : EventArgs
    {
        internal DiagnosticEventArgs(Diagnostic diagnostic)
        {
            Diagnostic = diagnostic;
        }

        public Diagnostic Diagnostic { get; }
    }
}
