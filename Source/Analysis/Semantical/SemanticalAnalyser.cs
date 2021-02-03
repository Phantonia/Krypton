namespace Krypton.Analysis.Semantical
{
    public sealed class SemanticalAnalyser
    {
        public SemanticalAnalyser(Compilation compilation)
        {
            Compilation = compilation;
        }

        public Compilation Compilation { get; }

        public bool PerformSemanticalAnalysis()
        {
            Binder binder = new(Compilation);

            bool success = binder.PerformBinding();

            if (!success)
            {
                return false;
            }

            TypeChecker typeChecker = new(Compilation, binder.TypeManager);

            success = typeChecker.PerformTypeChecking();

            if (!success)
            {
                return false;
            }

            return true;
        }
    }
}
