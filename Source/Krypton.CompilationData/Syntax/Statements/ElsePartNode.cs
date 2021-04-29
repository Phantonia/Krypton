using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record ElsePartNode : SyntaxNode
    {
        public ElsePartNode(ReservedKeywordToken elseKeyword,
                            BodyNode body)
        {
            ElseKeywordToken = elseKeyword;
            BodyNode = body;

            Debug.Assert(ElseKeywordToken.Keyword == ReservedKeyword.Else);
        }

        public BodyNode BodyNode { get; init; }

        public ReservedKeywordToken ElseKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public override void WriteCode(TextWriter output)
        {
            ElseKeywordToken.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
