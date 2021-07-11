using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed record BodyNode : SyntaxNode
    {
        public BodyNode(SyntaxCharacterToken? openingBrace,
                        IEnumerable<StatementNode>? statements,
                        SyntaxCharacterToken? closingBrace)
        {
            OpeningBraceToken = openingBrace;
            StatementNodes = statements?.ToImmutableList() ?? ImmutableList<StatementNode>.Empty;
            ClosingBraceToken = closingBrace;

            Debug.Assert(OpeningBraceToken == null || OpeningBraceToken.SyntaxCharacter == SyntaxCharacter.BraceOpening);
            Debug.Assert(ClosingBraceToken == null || ClosingBraceToken.SyntaxCharacter == SyntaxCharacter.BraceClosing);
        }

        public SyntaxCharacterToken? ClosingBraceToken { get; init; }

        public override bool IsLeaf => StatementNodes.Count == 0; // { } is a leaf

        public override Token LexicallyFirstToken => OpeningBraceToken ?? StatementNodes[0].LexicallyFirstToken;

        public ImmutableList<StatementNode> StatementNodes { get; init; }

        public SyntaxCharacterToken? OpeningBraceToken { get; init; }

        protected override string GetDebuggerDisplay()
            => $"{base.GetDebuggerDisplay()}; Count = {StatementNodes.Count}";

        public override void WriteCode(TextWriter output)
        {
            OpeningBraceToken?.WriteCode(output);

            foreach (StatementNode statement in StatementNodes)
            {
                statement.WriteCode(output);
            }

            ClosingBraceToken?.WriteCode(output);
        }
    }
}
