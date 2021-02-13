using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Semantical.IdentifierMaps;

namespace Krypton.Analysis.Semantical
{
    public sealed partial class Binder
    {
        public Binder(Compilation compilation)
        {
            Compilation = compilation;
        }

#nullable disable // these are assigned by the only method that calls others, PerformBinding()
        private HoistedIdentifierMap globalIdentifierMap;
        private TypeManager typeManager;
#nullable restore

        public Compilation Compilation { get; }

        public bool PerformBinding()
        {
            TypeIdentifierMap typeIdentifierMap = GatherGlobalTypes();
            typeManager = new TypeManager(Compilation, typeIdentifierMap);

            HoistedIdentifierMap? globalIdentifierMap = GatherGlobalSymbols();

            if (globalIdentifierMap == null)
            {
                return false;
            }

            this.globalIdentifierMap = globalIdentifierMap;

            {
                bool success = BindInTopLevelStatements();

                if (!success)
                {
                    return false;
                }
            }

            foreach (FunctionDeclarationNode function in Compilation.Program.Functions)
            {
                VariableIdentifierMap variableIdentifierMap = new();
                bool success = BindInStatementBlock(function.BodyNode, variableIdentifierMap);

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        private bool BindInTopLevelStatements()
        {
            VariableIdentifierMap variableIdentifierMap = new();

            bool success = BindInStatementBlock(Compilation.Program.TopLevelStatementNodes,
                                                variableIdentifierMap);

            return success;
        }
    }
}
