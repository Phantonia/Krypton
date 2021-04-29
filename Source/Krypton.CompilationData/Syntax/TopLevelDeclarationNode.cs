using Krypton.CompilationData.Syntax.Declarations;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed record TopLevelDeclarationNode : TopLevelNode
    {
        public TopLevelDeclarationNode(DeclarationNode declaration)
        {
            DeclarationNode = declaration;
        }

        public override bool IsLeaf => false;

        public DeclarationNode DeclarationNode { get; init; }

        public override void WriteCode(TextWriter output)
            => DeclarationNode.WriteCode(output);
    }
}
