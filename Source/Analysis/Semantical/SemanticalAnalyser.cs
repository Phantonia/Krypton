using Krypton.Analysis.AbstractSyntaxTree;

namespace Krypton.Analysis.Semantical
{
    public sealed class SemanticalAnalyser
    {
        public SemanticalAnalyser(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public bool PerformSemanticalAnalysis()
        {
            return true;
        }
    }
}
