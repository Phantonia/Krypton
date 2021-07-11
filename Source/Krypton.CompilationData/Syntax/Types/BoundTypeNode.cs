using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Types
{
    public sealed record BoundTypeNode : TypeNode
    {
        internal BoundTypeNode(TypeNode typeNode, TypeSymbol type)
        {
            TypeNode = typeNode;
            TypeSymbol = type;
        }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => TypeNode.LexicallyFirstToken;

        public TypeNode TypeNode { get; init; }

        public TypeSymbol TypeSymbol { get; init; }

        public override BoundTypeNode Bind(TypeSymbol typeSymbol)
            => typeSymbol == TypeSymbol ? this : new BoundTypeNode(TypeNode, typeSymbol);

        public override void WriteCode(TextWriter output)
            => TypeNode.WriteCode(output);
    }
}
