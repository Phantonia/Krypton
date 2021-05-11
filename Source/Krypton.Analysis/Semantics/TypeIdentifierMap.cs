using Krypton.CompilationData.Symbols;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantics
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class TypeIdentifierMap
    {
        public TypeIdentifierMap() { }

        private readonly Dictionary<FrameworkType, string> frameworkTypes = new(capacity: 6);

        private readonly Dictionary<string, TypeSymbol> typeNodes = new();

        public TypeSymbol this[string identifier] => typeNodes[identifier];

        public TypeSymbol this[FrameworkType frameworkType] => typeNodes[frameworkTypes[frameworkType]];

        public bool AddSymbol(string identifier, TypeSymbol type)
        {
            return typeNodes.TryAdd(identifier, type);
        }

        public bool AddSymbol(string identifier, FrameworkType frameworkType, TypeSymbol typeNode)
        {
            frameworkTypes[frameworkType] = identifier;
            return AddSymbol(identifier, typeNode);
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out TypeSymbol? type)
        {
            return typeNodes.TryGetValue(identifier, out type);
        }

        public bool TryGet(FrameworkType frameworkType, [NotNullWhen(true)] out TypeSymbol? type)
        {
            type = null;
            return frameworkTypes.TryGetValue(frameworkType, out string? key) && typeNodes.TryGetValue(key, out type);
        }

        private string GetDebuggerDisplay()
        {
            return $"{GetType().Name}; Count = {typeNodes.Count}";
        }
    }
}
