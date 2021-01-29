using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical.IdentifierMaps
{
    public sealed class TypeIdentifierMap
    {
        public TypeIdentifierMap() { }

        private readonly Dictionary<string, TypeSymbolNode> types = new();

        public TypeSymbolNode this[string identifier] => types[identifier];

        public bool AddSymbol(string identifier, TypeSymbolNode type)
        {
            return types.TryAdd(identifier, type);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out TypeSymbolNode? type)
        {
            return types.TryGetValue(identifier, out type);
        }
    }
}
