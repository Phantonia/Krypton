using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record LoopControlStatementNode : SingleStatementNode
    {
        public LoopControlStatementNode(ReservedKeywordToken keyword,
                                        LiteralToken<long>? level,
                                        SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            Debug.Assert(keyword.Keyword is ReservedKeyword.Leave or ReservedKeyword.Continue);
            Debug.Assert(level == null || level.Value > 0);

            KeywordToken = keyword;
            LevelToken = level;
        }

        public bool IsContinue => KeywordToken.Keyword == ReservedKeyword.Continue;

        public override bool IsLeaf => true;

        public bool IsLeave => KeywordToken.Keyword == ReservedKeyword.Leave;

        public ReservedKeywordToken KeywordToken { get; init; }

        public int Level => (int?)LevelToken?.Value ?? 1;

        public LiteralToken<long>? LevelToken { get; init; }

        public override Token LexicallyFirstToken => KeywordToken;

        protected override string GetDebuggerDisplay()
            => $"{base.GetDebuggerDisplay()}; Level: {Level}";

        public override void WriteCode(TextWriter output)
        {
            KeywordToken.WriteCode(output);
            LevelToken?.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
