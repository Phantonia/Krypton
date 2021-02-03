using Krypton.Analysis.Ast.Symbols;
using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical.IdentifierMaps
{
    public sealed class TypeIdentifierMap
    {
        public TypeIdentifierMap() { }

        private readonly Dictionary<FrameworkType, string> frameworkTypes = new(capacity: 6);

        private readonly Dictionary<string, TypeSymbolNode> typeNodes = new();

        public TypeSymbolNode this[string identifier] => typeNodes[identifier];

        public TypeSymbolNode this[FrameworkType frameworkType] => typeNodes[frameworkTypes[frameworkType]];

        public bool AddSymbol(string identifier, TypeSymbolNode type)
        {
            return typeNodes.TryAdd(identifier, type);
        }

        public bool AddSymbol(string identifier, FrameworkType frameworkType, TypeSymbolNode typeNode)
        {
            frameworkTypes[frameworkType] = identifier;
            return AddSymbol(identifier, typeNode);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out TypeSymbolNode? type)
        {
            return typeNodes.TryGetValue(identifier, out type);
        }

        public bool TryGet(FrameworkType frameworkType, [NotNullWhen(true)] out TypeSymbolNode? type)
        {
            type = null;
            return frameworkTypes.TryGetValue(frameworkType, out string? key) && typeNodes.TryGetValue(key, out type);
        }
    }
}
