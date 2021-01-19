using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.Semantical.Binding
{
    public sealed class GlobalIdentifierMap : IIdentifierMap
    {
        public GlobalIdentifierMap() { }

        private readonly Dictionary<string, SymbolNode> symbols = new();

        public SymbolNode? this[string identifier] => symbols.TryGetValue(identifier, out SymbolNode? symbol) ? symbol : null;

        public bool AddSymbol(string identifier, SymbolNode symbol) => symbols.TryAdd(identifier, symbol);
    }
}