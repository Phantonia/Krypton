using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Symbols;

namespace Krypton.Analysis.Semantical
{
    internal sealed partial class Binder
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
                bool success = BindInFunction(function);

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        private bool BindInFunction(FunctionDeclarationNode function)
        {
            VariableIdentifierMap variableIdentifierMap = new();

            foreach (ParameterDeclarationNode parameter in function.ParameterNodes)
            {
                TypeSymbolNode? typeSymbol = GetTypeSymbol(parameter.TypeNode);

                if (typeSymbol == null)
                {
                    return false;
                }

                VariableSymbolNode parameterVariable = new(parameter.Identifier,
                                                           typeSymbol,
                                                           isReadOnly: false,
                                                           parameter.LineNumber,
                                                           parameter.Index);

                variableIdentifierMap.AddSymbol(parameter.Identifier, parameterVariable);
            }

            bool success = BindInStatementBlock(function.BodyNode, variableIdentifierMap);
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
