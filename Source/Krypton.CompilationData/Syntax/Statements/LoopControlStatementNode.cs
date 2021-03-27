using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class LoopControlStatementNode : SingleStatementNode
    {
        public LoopControlStatementNode(ReservedKeywordToken keyword,
                                        LiteralToken<long>? level,
                                        SyntaxCharacterToken semicolon,
                                        SyntaxNode? parent = null)
            : base(semicolon, parent)
        {
            Debug.Assert(keyword.Keyword is ReservedKeyword.Leave or ReservedKeyword.Continue);
            Debug.Assert(level == null || level.Value > 0);

            KeywordToken = keyword;
            LevelToken = level;
        }

        public bool IsContinue => KeywordToken.Keyword == ReservedKeyword.Continue;

        public override bool IsLeaf => true;

        public bool IsLeave => KeywordToken.Keyword == ReservedKeyword.Leave;

        public ReservedKeywordToken KeywordToken { get; }

        public long Level => LevelToken?.Value ?? 1L;

        public LiteralToken<long>? LevelToken { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Level: {Level}";

        public LoopControlStatementNode WithChildren(ReservedKeywordToken? keyword = null,
                                                     LiteralToken<long>? level = null,
                                                     bool overwriteLevel = false,
                                                     SyntaxCharacterToken? semicolon = null)
            => new(keyword ?? KeywordToken,
                   level ?? (overwriteLevel ? null : LevelToken),
                   semicolon ?? SemicolonToken);

        public override LoopControlStatementNode WithParent(SyntaxNode newParent)
            => new(KeywordToken, LevelToken, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            KeywordToken.WriteCode(output);
            LevelToken?.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
