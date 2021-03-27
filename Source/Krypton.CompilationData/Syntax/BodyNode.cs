﻿using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax
{
    public sealed class BodyNode : SyntaxNode
    {
        public BodyNode(SyntaxCharacterToken openingBraceToken,
                        IEnumerable<StatementNode> statementNodes,
                        SyntaxCharacterToken closingBraceToken,
                        SyntaxNode? parent = null)
            : base(parent)
        {
            OpeningBraceToken = openingBraceToken;
            StatementNodes = statementNodes.Select(s => s.WithParent(this)).MakeReadOnly();
            ClosingBraceToken = closingBraceToken;

            Debug.Assert(OpeningBraceToken.SyntaxCharacter == SyntaxCharacter.BraceOpening);
            Debug.Assert(ClosingBraceToken.SyntaxCharacter == SyntaxCharacter.BraceClosing);
        }

        public SyntaxCharacterToken ClosingBraceToken { get; }

        public override bool IsLeaf => StatementNodes.Count == 0; // { } is a leaf

        public ReadOnlyList<StatementNode> StatementNodes { get; }

        public SyntaxCharacterToken OpeningBraceToken { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Count = {StatementNodes.Count}";

        public BodyNode WithChildren(SyntaxCharacterToken? openingBraceToken = null,
                                     IEnumerable<StatementNode>? statementNodes = null,
                                     IndexWither<StatementNode>[]? statementNodeWithers = null,
                                     SyntaxCharacterToken? closingBraceToken = null)
            => new(openingBraceToken ?? OpeningBraceToken,
                   statementNodes ?? StatementNodes.With(statementNodeWithers),
                   closingBraceToken ?? ClosingBraceToken);

        public override BodyNode WithParent(SyntaxNode newParent)
            => new(OpeningBraceToken, StatementNodes, ClosingBraceToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            OpeningBraceToken.WriteCode(output);

            foreach (StatementNode statement in StatementNodes)
            {
                statement.WriteCode(output);
            }

            ClosingBraceToken.WriteCode(output);
        }
    }
}