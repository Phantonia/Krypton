using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(IdentifierToken identifier)
        {
            IdentifierToken = identifier;
        }

        public ReadOnlyMemory<char> Identifier => IdentifierToken.Text;

        public IdentifierToken IdentifierToken { get; init; }

        public override bool IsLeaf => true;

        public override Token LexicallyFirstToken => IdentifierToken;

        public BoundExpressionNode<IdentifierExpressionNode, Symbol> Bind(Symbol symbol)
            => new(this, symbol);

        public override void WriteCode(TextWriter output)
        {
            IdentifierToken.WriteCode(output);
        }
    }
}
