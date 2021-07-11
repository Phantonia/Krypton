using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System;
using System.IO;

namespace Krypton.CompilationData.Syntax.Types
{
    public sealed record IdentifierTypeNode : TypeNode
    {
        public IdentifierTypeNode(IdentifierToken identifier)
        {
            IdentifierToken = identifier;
        }

        public ReadOnlyMemory<char> Identifier => IdentifierToken.Text;

        public IdentifierToken IdentifierToken { get; init; }

        public override bool IsLeaf => true;

        public override Token LexicallyFirstToken => IdentifierToken;

        public override BoundTypeNode Bind(TypeSymbol typeSymbol)
            => new(this, typeSymbol);

        protected override string GetDebuggerDisplay()
            => $"{base.GetDebuggerDisplay()}; Identifier = {Identifier}";

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
