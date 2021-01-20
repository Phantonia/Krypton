using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical.IdentifierMaps
{
    public sealed class HoistedIdentifierMap
    {
        public HoistedIdentifierMap() { }

        private readonly Dictionary<string, SymbolNode> symbols = new();

        public bool AddSymbol(string identifier, SymbolNode symbol)
        {
            return symbols.TryAdd(identifier, symbol);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out SymbolNode? symbol)
        {
            return symbols.TryGetValue(identifier, out symbol);
        }
    }
}
