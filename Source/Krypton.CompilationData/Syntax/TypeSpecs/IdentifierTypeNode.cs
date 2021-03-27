using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Types
{
    public sealed class IdentifierTypeNode : TypeNode
    {
        public IdentifierTypeNode(IdentifierToken identifier)
            : this(identifier, parent: null) { }

        public IdentifierTypeNode(IdentifierToken identifierToken, SyntaxNode? parent)
            : base(parent)
        {
            IdentifierToken = identifierToken;
        }

        public IdentifierToken IdentifierToken { get; }

        public override bool IsLeaf => true;

        public override IdentifierTypeNode WithParent(SyntaxNode newParent)
            => new(IdentifierToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
