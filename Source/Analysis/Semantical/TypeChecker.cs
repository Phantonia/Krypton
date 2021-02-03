namespace Krypton.Analysis.Semantical
{
    public sealed partial class TypeChecker
    {
        public TypeChecker(Compilation compilation, TypeManager typeManager)
        {
            Compilation = compilation;
            this.typeManager = typeManager;
        }

        private readonly TypeManager typeManager;

        public Compilation Compilation { get; }

        public bool PerformTypeChecking()
        {
            bool success = CheckTopLevelStatements();

            if (!success)
            {
                return false;
            }

            return true;
        }

        private bool CheckTopLevelStatements()
        {
            return CheckStatementCollection(Compilation.Program.TopLevelStatementNodes);
        }
    }
}
