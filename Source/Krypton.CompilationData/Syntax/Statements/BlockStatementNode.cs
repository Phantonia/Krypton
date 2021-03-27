using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class BlockStatementNode : BodiedStatementNode
    {
        public BlockStatementNode(ReservedKeywordToken blockKeyword,
                                  BodyNode body,
                                  SyntaxNode? parent = null)
            : base(body, parent)
        {
            BlockKeywordToken = blockKeyword;
        }

        public ReservedKeywordToken BlockKeywordToken { get; }

        public override bool IsLeaf => false;

        public BlockStatementNode WithChildren(ReservedKeywordToken? blockKeyword = null,
                                               BodyNode? body = null)
            => new(blockKeyword ?? BlockKeywordToken,
                   body ?? BodyNode);

        public override BlockStatementNode WithParent(SyntaxNode newParent)
            => new(BlockKeywordToken, BodyNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            BlockKeywordToken.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
