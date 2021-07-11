using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed record BoundDeclarationNode : DeclarationNode
    {
        public BoundDeclarationNode(DeclarationNode declaration, Symbol symbol)
        {
            DeclarationNode = declaration;
            Symbol = symbol;
        }

        public DeclarationNode DeclarationNode { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => DeclarationNode.LexicallyFirstToken;

        public Symbol Symbol { get; init; }

        public override BoundDeclarationNode Bind(Symbol symbol)
            => symbol == Symbol ? this : new BoundDeclarationNode(DeclarationNode, symbol);

        public override void WriteCode(TextWriter output)
            => DeclarationNode.WriteCode(output);
    }
}
