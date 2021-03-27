using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Clauses
{
    public sealed class AsClauseNode : ClauseNode
    {
        public AsClauseNode(ReservedKeywordToken asKeyword,
                            TypeNode type,
                            SyntaxNode? parent = null)
            : base(parent)
        {
            Debug.Assert(asKeyword.Keyword == ReservedKeyword.As);

            AsKeywordToken = asKeyword;
            TypeNode = type.WithParent(this);
        }

        public ReservedKeywordToken AsKeywordToken { get; }

        public override bool IsLeaf => false;

        public TypeNode TypeNode { get; }

        public AsClauseNode WithChildren(ReservedKeywordToken? asKeyword = null,
                                         TypeNode? type = null)
            => new(asKeyword ?? AsKeywordToken,
                   type ?? TypeNode);

        public override AsClauseNode WithParent(SyntaxNode newParent)
            => new(AsKeywordToken, TypeNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            AsKeywordToken.WriteCode(output);
            TypeNode.WriteCode(output);
        }
    }
}
