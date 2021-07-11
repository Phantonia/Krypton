using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record PropertyGetAccessExpressionNode : ExpressionNode
    {
        public PropertyGetAccessExpressionNode(ExpressionNode source,
                                               SyntaxCharacterToken dot,
                                               IdentifierToken property)
        {
            Debug.Assert(dot.SyntaxCharacter == SyntaxCharacter.Dot);

            SourceNode = source;
            DotToken = dot;
            PropertyToken = property;
        }

        public SyntaxCharacterToken DotToken { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => SourceNode.LexicallyFirstToken;

        public IdentifierToken PropertyToken { get; init; }

        public ExpressionNode SourceNode { get; init; }

        public BoundExpressionNode<PropertyGetAccessExpressionNode, PropertySymbol> Bind(PropertySymbol symbol)
            => new(this, symbol);

        public override void WriteCode(TextWriter output)
        {
            SourceNode.WriteCode(output);
            DotToken.WriteCode(output);
            PropertyToken.WriteCode(output);
        }
    }
}
