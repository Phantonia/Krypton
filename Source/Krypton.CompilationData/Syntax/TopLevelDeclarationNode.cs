using Krypton.CompilationData.Syntax.Declarations;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed class TopLevelDeclarationNode : TopLevelNode
    {
        public TopLevelDeclarationNode(DeclarationNode declaration,
                                       SyntaxNode? parent = null)
            : base(parent)
        {
            DeclarationNode = declaration;
        }

        public override bool IsLeaf => false;

        public DeclarationNode DeclarationNode { get; }

        public override TopLevelDeclarationNode WithParent(SyntaxNode newParent)
            => new(DeclarationNode, newParent);

        public override void WriteCode(TextWriter output) => DeclarationNode.WriteCode(output);
    }
}
