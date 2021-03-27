using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ElsePartNode : SyntaxNode
    {
        public ElsePartNode(ReservedKeywordToken elseKeyword,
                            BodyNode body,
                            SyntaxNode? parent = null)
            : base(parent)
        {
            ElseKeywordToken = elseKeyword;
            BodyNode = body.WithParent(this);

            Debug.Assert(ElseKeywordToken.Keyword == ReservedKeyword.Else);
        }

        public BodyNode BodyNode { get; }

        public ReservedKeywordToken ElseKeywordToken { get; }

        public override bool IsLeaf => false;

        public ElsePartNode WithChildren(ReservedKeywordToken? elseKeyword = null,
                                         BodyNode? body = null)
            => new(elseKeyword ?? ElseKeywordToken,
                   body ?? BodyNode);

        public override ElsePartNode WithParent(SyntaxNode newParent)
            => new(ElseKeywordToken, BodyNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            ElseKeywordToken.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
