using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Types;
using Krypton.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical
{
    internal sealed class TypeManager
    {
        public TypeManager(ProgramNode program, TypeIdentifierMap typeIdentifierMap)
        {
            this.program = program;
            this.typeIdentifierMap = typeIdentifierMap;
        }

        private readonly ProgramNode program;
        private readonly TypeIdentifierMap typeIdentifierMap;

        public TypeSymbol this[FrameworkType frameworkType] => typeIdentifierMap[frameworkType];

        public bool TryGetTypeSymbol(TypeNode? typeNode, out TypeSymbol typeSymbol)
        {
            if (typeNode == null)
            {
                typeSymbol = TypeSymbol.VoidType;
                return true;
            }

            if (typeNode is IdentifierTypeNode identifierTypeSpecNode)
            {
                if (typeIdentifierMap.TryGet(identifierTypeSpecNode.IdentifierToken.TextToString(), out typeSymbol!))
                {
                    identifierTypeSpecNode.Bind(typeSymbol);
                    return true;
                }

                return false;
            }
            else
            {
                Debug.Fail(message: null);
                typeSymbol = null;
                return false;
            }
        }

        public bool TryGetTypeSymbol(FrameworkType frameworkType, [NotNullWhen(true)] out TypeSymbol? typeSymbol)
        {
            return typeIdentifierMap.TryGet(frameworkType, out typeSymbol);
        }
    }
}
