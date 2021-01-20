using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using System;

namespace Krypton.Analysis.Semantical
{
    public sealed class TypeManager
    {
        public TypeManager(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
        }

        private readonly SyntaxTree syntaxTree;

        public bool TryGetTypeSymbol(TypeSpecNode? typeSpec, out TypeSymbolNode? typeSymbol)
        {
            if (typeSpec == null)
            {
                typeSymbol = null;
                return true;
            }

            throw new NotImplementedException();
        }
    }
}
