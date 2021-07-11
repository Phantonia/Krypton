using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record IfStatementNode : BodiedStatementNode
    {
        public IfStatementNode(ReservedKeywordToken ifKeyword,
                               ExpressionNode condition,
                               BodyNode body,
                               IEnumerable<ElseIfPartNode>? elseIfParts,
                               ElsePartNode? elsePart)
            : base(body)
        {
            IfKeywordToken = ifKeyword;
            ConditionNode = condition;
            ElseIfPartNodes = elseIfParts?.ToImmutableList() ?? ImmutableList<ElseIfPartNode>.Empty;
            ElsePartNode = elsePart;

            Debug.Assert(IfKeywordToken.Keyword == ReservedKeyword.If);
        }

        public ExpressionNode ConditionNode { get; init; }

        public ImmutableList<ElseIfPartNode> ElseIfPartNodes { get; init; }

        public ElsePartNode? ElsePartNode { get; init; }

        public ReservedKeywordToken IfKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => IfKeywordToken;

        public override void WriteCode(TextWriter output)
        {
            IfKeywordToken.WriteCode(output);
            ConditionNode.WriteCode(output);
            BodyNode.WriteCode(output);

            foreach (ElseIfPartNode part in ElseIfPartNodes)
            {
                part.WriteCode(output);
            }

            ElsePartNode?.WriteCode(output);
        }
    }
}
