using Krypton.CompilationData.Symbols;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.CompilationData
{
    public sealed class SymbolTable
    {
        public SymbolTable(IEnumerable<KeyValuePair<string, Symbol>> symbols)
        {
            this.symbols = symbols.ToImmutableDictionary();
        }

        public SymbolTable(ImmutableDictionary<string, Symbol> symbols)
        {
            this.symbols = symbols;
        }

        private readonly ImmutableDictionary<string, Symbol> symbols;

        public Symbol this[string name] => symbols[name];

        public SymbolTable Add(string name, Symbol symbol)
            => new(symbols.Add(name, symbol));

        public SymbolTable AddMany(IEnumerable<KeyValuePair<string, Symbol>> range)
            => new(symbols.AddRange(range));

        public bool ContainsName(string name)
            => symbols.ContainsKey(name);

        public bool TryGetSymbol(string name, [NotNullWhen(true)] out Symbol? symbol)
            => symbols.TryGetValue(name, out symbol);

        public bool TryGetSymbol<TSymbol>(string name, [NotNullWhen(true)] out TSymbol? symbol)
            where TSymbol : Symbol
        {
            if (!symbols.ContainsKey(name))
            {
                symbol = null;
                return false;
            }

            symbol = symbols[name] as TSymbol;
            return symbol != null;
        }
    }
}
