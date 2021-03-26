using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.TypeSpecs
{
    public sealed class IdentifierTypeSpecNode : TypeSpecNode
    {
        public IdentifierTypeSpecNode(IdentifierToken identifierToken)
            : this(identifierToken, parent: null) { }

        public IdentifierTypeSpecNode(IdentifierToken identifierToken, SyntaxNode? parent)
            : base(parent)
        {
            IdentifierToken = identifierToken;
        }

        public IdentifierToken IdentifierToken { get; }

        public override bool IsLeaf => true;

        public override IdentifierTypeSpecNode WithParent(SyntaxNode newParent)
            => new(IdentifierToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
