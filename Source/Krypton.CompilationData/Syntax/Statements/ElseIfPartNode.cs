using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class ElseIfPartNode : SyntaxNode
    {
        public ElseIfPartNode(ReservedKeywordToken elseKeyword,
                              ReservedKeywordToken ifKeyword,
                              ExpressionNode condition,
                              BodyNode body,
                              SyntaxNode? parent = null)
            : base(parent)
        {
            ElseKeywordToken = elseKeyword;
            IfKeywordToken = ifKeyword;
            ConditionNode = condition.WithParent(this);
            BodyNode = body.WithParent(this);

            Debug.Assert(ElseKeywordToken.Keyword == ReservedKeyword.Else);
            Debug.Assert(IfKeywordToken.Keyword == ReservedKeyword.If);
        }

        public BodyNode BodyNode { get; }

        public ExpressionNode ConditionNode { get; }

        public ReservedKeywordToken ElseKeywordToken { get; }

        public ReservedKeywordToken IfKeywordToken { get; }

        public override bool IsLeaf => false;

        public ElseIfPartNode WithChildren(ReservedKeywordToken? elseKeyword = null,
                                           ReservedKeywordToken? ifKeyword = null,
                                           ExpressionNode? condition = null,
                                           BodyNode? body = null)
            => new(elseKeyword ?? ElseKeywordToken,
                   ifKeyword ?? IfKeywordToken,
                   condition ?? ConditionNode,
                   body ?? BodyNode);

        public override ElseIfPartNode WithParent(SyntaxNode newParent)
            => new(ElseKeywordToken, IfKeywordToken, ConditionNode, BodyNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            ElseKeywordToken.WriteCode(output);
            IfKeywordToken.WriteCode(output);
            ConditionNode.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
