using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Types
{
    public sealed class IdentifierTypeNode : TypeNode
    {
        public IdentifierTypeNode(IdentifierToken identifier,
                                  SyntaxNode? parent = null)
            : base(parent)
        {
            IdentifierToken = identifier;
        }

        public string Identifier => IdentifierToken.Text;

        public IdentifierToken IdentifierToken { get; }

        public override bool IsLeaf => true;

        public override BoundTypeNode<IdentifierTypeNode> Bind(TypeSymbol typeSymbol)
            => new(this, typeSymbol);

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Identifier = {Identifier}";

        public override IdentifierTypeNode WithParent(SyntaxNode newParent)
            => new(IdentifierToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
