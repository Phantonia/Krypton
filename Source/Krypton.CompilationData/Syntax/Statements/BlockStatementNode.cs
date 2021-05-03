using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record BlockStatementNode : BodiedStatementNode
    {
        public BlockStatementNode(ReservedKeywordToken blockKeyword,
                                  BodyNode body)
            : base(body)
        {
            BlockKeywordToken = blockKeyword;
        }

        public ReservedKeywordToken BlockKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public override void WriteCode(TextWriter output)
        {
            BlockKeywordToken.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
