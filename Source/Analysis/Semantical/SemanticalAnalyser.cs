using Krypton.Analysis.AST;
using Krypton.Analysis.Semantical.Binding;

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
            IBinder binder = new TypeAgnosticBinder(SyntaxTree, new BuiltinIdentifierMap());
            return binder.PerformBinding();
        }
    }
}
