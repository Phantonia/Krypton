using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Semantical.IdentifierMaps;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    public sealed class TypeManager
    {
        public TypeManager(SyntaxTree syntaxTree, TypeIdentifierMap typeIdentifierMap)
        {
            this.syntaxTree = syntaxTree;
            this.typeIdentifierMap = typeIdentifierMap;
        }

        private readonly SyntaxTree syntaxTree;
        private readonly TypeIdentifierMap typeIdentifierMap;

        public bool TryGetTypeSymbol(TypeSpecNode? typeSpec, out TypeSymbolNode? typeSymbol)
        {
            if (typeSpec == null)
            {
                typeSymbol = null;
                return true;
            }

            if (typeSpec is IdentifierTypeSpecNode idtn)
            {
                if (typeIdentifierMap.TryGet(idtn.Identifier, out typeSymbol))
                {
                    idtn.Bind(typeSymbol);
                    return true;
                }

                return false;
            }
            else
            {
                Debug.Fail(null);
                typeSymbol = null;
                return false;
            }
        }
    }
}
