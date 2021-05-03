using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Clauses
{
    public sealed record AsClauseNode : ClauseNode
    {
        public AsClauseNode(ReservedKeywordToken asKeyword,
                            TypeNode type)
        {
            Debug.Assert(asKeyword.Keyword == ReservedKeyword.As);

            AsKeywordToken = asKeyword;
            TypeNode = type;
        }

        public ReservedKeywordToken AsKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public TypeNode TypeNode { get; init; }

        public override void WriteCode(TextWriter output)
        {
            AsKeywordToken.WriteCode(output);
            TypeNode.WriteCode(output);
        }
    }
}
