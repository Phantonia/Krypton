using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class IfStatementNode : BodiedStatementNode
    {
        public IfStatementNode(ReservedKeywordToken ifKeyword,
                               ExpressionNode condition,
                               BodyNode body,
                               IEnumerable<ElseIfPartNode>? elseIfParts,
                               ElsePartNode? elsePart,
                               SyntaxNode? parent = null)
            : base(body, parent)
        {
            IfKeywordToken = ifKeyword;
            ConditionNode = condition.WithParent(this);
            ElseIfPartNodes = elseIfParts?.Select(p => p.WithParent(this)).Finalize() ?? default;
            ElsePartNode = elsePart?.WithParent(this);

            Debug.Assert(IfKeywordToken.Keyword == ReservedKeyword.If);
        }

        public ExpressionNode ConditionNode { get; }

        public FinalList<ElseIfPartNode> ElseIfPartNodes { get; }

        public ElsePartNode? ElsePartNode { get; }

        public ReservedKeywordToken IfKeywordToken { get; }

        public override bool IsLeaf => false;

        public IfStatementNode WithChildren(ReservedKeywordToken? ifKeyword = null,
                                            ExpressionNode? condition = null,
                                            BodyNode? body = null,
                                            IEnumerable<ElseIfPartNode>? elseIfParts = null,
                                            bool overwriteElseIfParts = false,
                                            ElsePartNode? elsePart = null,
                                            bool overwriteElsePart = false)
            => new(ifKeyword ?? IfKeywordToken,
                   condition ?? ConditionNode,
                   body ?? BodyNode,
                   elseIfParts ?? (overwriteElseIfParts ? null : ElseIfPartNodes),
                   elsePart ?? (overwriteElsePart ? null : ElsePartNode));

        public override IfStatementNode WithParent(SyntaxNode newParent)
            => new(IfKeywordToken, ConditionNode, BodyNode, ElseIfPartNodes, ElsePartNode, newParent);

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
