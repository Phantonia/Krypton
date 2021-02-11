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
            (HoistedIdentifierMap globalIdentifierMap, TypeIdentifierMap typeIdentifierMap) = GatherGlobalSymbols();
            this.globalIdentifierMap = globalIdentifierMap;
            typeManager = new TypeManager(Compilation, typeIdentifierMap);

            bool success = BindInTopLevelStatements();
            return success;
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
