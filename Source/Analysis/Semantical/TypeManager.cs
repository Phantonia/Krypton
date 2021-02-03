using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical
{
    public sealed class TypeManager
    {
        public TypeManager(Compilation compilation, TypeIdentifierMap typeIdentifierMap)
        {
            this.compilation = compilation;
            this.typeIdentifierMap = typeIdentifierMap;
        }

        private readonly Compilation compilation;
        private readonly TypeIdentifierMap typeIdentifierMap;

        public TypeSymbolNode this[FrameworkType frameworkType] => typeIdentifierMap[frameworkType];

        public bool TryGetTypeSymbol(TypeSpecNode? typeSpec, out TypeSymbolNode? typeSymbol)
        {
            if (typeSpec == null)
            {
                typeSymbol = null;
                return true;
            }

            if (typeSpec is IdentifierTypeSpecNode identifierTypeSpecNode)
            {
                if (typeIdentifierMap.TryGet(identifierTypeSpecNode.Identifier, out typeSymbol))
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

        public bool TryGetTypeSymbol(FrameworkType frameworkType, [NotNullWhen(true)] out TypeSymbolNode? typeSymbol)
        {
            return typeIdentifierMap.TryGet(frameworkType, out typeSymbol);
        }
    }
}
