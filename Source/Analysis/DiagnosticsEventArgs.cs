using Krypton.CompilationData;
using System;

namespace Krypton.Analysis
{
    public delegate void DiagnosticsEventHandler(object? sender, DiagnosticsEventArgs e);

    public sealed class DiagnosticsEventArgs : EventArgs
    {
        internal DiagnosticsEventArgs(Diagnostic diagnostic)
        {
            Diagnostic = diagnostic;
        }

        public Diagnostic Diagnostic { get; }
    }
}
