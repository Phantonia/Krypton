using Krypton.CompilationData.Syntax.Declarations;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed record TopLevelDeclarationNode : TopLevelNode
    {
        public TopLevelDeclarationNode(DeclarationNode declaration)
        {
            DeclarationNode = declaration;
        }

        public DeclarationNode DeclarationNode { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => DeclarationNode.LexicallyFirstToken;

        public override void WriteCode(TextWriter output)
            => DeclarationNode.WriteCode(output);
    }
}
