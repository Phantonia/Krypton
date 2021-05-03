using Krypton.CompilationData.Symbols;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class HoistedIdentifierMap
    {
        public HoistedIdentifierMap() { }

        private readonly Dictionary<string, Symbol> symbolNodes = new();

        public Symbol this[string identifier] => symbolNodes[identifier];

        public bool AddSymbol(string identifier, Symbol symbol)
        {
            return symbolNodes.TryAdd(identifier, symbol);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out Symbol? symbol)
        {
            return symbolNodes.TryGetValue(identifier, out symbol);
        }

        private string GetDebuggerDisplay()
        {
            return $"{GetType().Name}; Count = {symbolNodes.Count}";
        }
    }
}
