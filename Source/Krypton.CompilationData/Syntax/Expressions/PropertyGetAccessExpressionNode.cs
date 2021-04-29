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

        public SyntaxCharacterToken DotToken { get; }

        public override bool IsLeaf => false;

        public IdentifierToken PropertyToken { get; }

        public ExpressionNode SourceNode { get; }

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

        public override void WriteCode(TextWriter output)
        {
            SourceNode.WriteCode(output);
            DotToken.WriteCode(output);
            PropertyToken.WriteCode(output);
        }
    }
}
