using Krypton.Analysis.Ast;

namespace Krypton.Analysis.Semantical
{
    public sealed partial class TypeChecker
    {
        public TypeChecker(SyntaxTree syntaxTree, TypeManager typeManager)
        {
            SyntaxTree = syntaxTree;
            this.typeManager = typeManager;
        }

        private readonly TypeManager typeManager;

        public SyntaxTree SyntaxTree { get; }

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
            return CheckStatementCollection(SyntaxTree.Root.TopLevelStatements);
        }
    }
}
