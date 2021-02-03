using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical.IdentifierMaps
{
    public sealed class HoistedIdentifierMap
    {
        public HoistedIdentifierMap() { }

        private readonly Dictionary<string, SymbolNode> symbolNodes = new();

        public SymbolNode this[string identifier] => symbolNodes[identifier];

        public bool AddSymbol(string identifier, SymbolNode symbol)
        {
            return symbolNodes.TryAdd(identifier, symbol);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out SymbolNode? symbol)
        {
            return symbolNodes.TryGetValue(identifier, out symbol);
        }
    }
}
