using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record ElseIfPartNode : SyntaxNode
    {
        public ElseIfPartNode(ReservedKeywordToken elseKeyword,
                              ReservedKeywordToken ifKeyword,
                              ExpressionNode condition,
                              BodyNode body)
        {
            ElseKeywordToken = elseKeyword;
            IfKeywordToken = ifKeyword;
            ConditionNode = condition;
            BodyNode = body;

            Debug.Assert(ElseKeywordToken.Keyword == ReservedKeyword.Else);
            Debug.Assert(IfKeywordToken.Keyword == ReservedKeyword.If);
        }

        public BodyNode BodyNode { get; init; }

        public ExpressionNode ConditionNode { get; init; }

        public ReservedKeywordToken ElseKeywordToken { get; init; }

        public ReservedKeywordToken IfKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => ElseKeywordToken;

        public override void WriteCode(TextWriter output)
        {
            ElseKeywordToken.WriteCode(output);
            IfKeywordToken.WriteCode(output);
            ConditionNode.WriteCode(output);
            BodyNode.WriteCode(output);
        }
    }
}
