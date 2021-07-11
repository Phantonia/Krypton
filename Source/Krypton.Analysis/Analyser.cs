﻿#pragma warning disable IDE0005 // Using directive is unnecessary.
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Semantics;
using Krypton.Analysis.Syntax;
using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
#pragma warning restore IDE0005 // Using directive is unnecessary.

[assembly: InternalsVisibleTo("Krypton.Tests")]
[assembly: InternalsVisibleTo("Krypton.Tests.Analysis")]
namespace Krypton.Analysis
{
    public sealed class Analyser
    {
        public Analyser(string code)
        {
            this.code = code;
            Diagnostics = new ReadOnlyCollection<Diagnostic>(diagnostics);
        }

        private readonly string code;
        private readonly List<Diagnostic> diagnostics = new();
        private readonly DiagnosticsMessageProvider messageProvider = new();

        internal ReadOnlyCollection<Diagnostic> Diagnostics { get; }

        public event DiagnosticsEventHandler? DiagnosticReported;

        //public Compilation? Analyse()
        //{
        //    Lexer lexer = new(code, this);
        //    FinalList<Token>? tokens = lexer.LexAll();

        //    if (tokens is not { } nonNullTokens)
        //    {
        //        return null;
        //    }

        //    ProgramParser parser = new(nonNullTokens, this);
        //    ProgramNode? program = parser.ParseWholeProgram();

        //    if (program == null)
        //    {
        //        return null;
        //    }

        //    Binder binder = new(program, this);
        //    BindingResult? result = binder.PerformBinding();

        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    return new Compilation(program, symbols: null, diagnostics);
        //}

        internal void ReportDiagnostic(Diagnostic diagnostic)
        {
            DiagnosticsEventArgs e = new(diagnostic);
            DiagnosticReported?.Invoke(this, e);
            diagnostics.Add(diagnostic);
        }

        internal void ReportError(DiagnosticsCode errorCode, Token offendingToken, params string[] details)
            => ReportError(errorCode, offendingToken, offendingNode: null, details);

        internal void ReportError(DiagnosticsCode errorCode, Token offendingToken, SyntaxNode? offendingNode, params string[] details)
        {
            string messageString = messageProvider[errorCode];

            Diagnostic error = new(errorCode,
                                   messageString,
                                   details.ToImmutableArray(),
                                   CodeLine: string.Empty,
                                   LineIndex: 0,
                                   IsError: true,
                                   offendingToken,
                                   offendingNode);

            ReportDiagnostic(error);
        }
    }
}