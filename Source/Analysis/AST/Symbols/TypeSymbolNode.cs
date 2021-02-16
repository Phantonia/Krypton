using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class TypeSymbolNode : SymbolNode
    {
        private protected TypeSymbolNode(string name, int lineNumber, int index) : base(name, lineNumber, index) { }

        public ReadOnlyDictionary<string, PropertySymbolNode> PropertyNodes { get; private set; }

        internal void SetProperties(IDictionary<string, PropertySymbolNode> properties)
            => PropertyNodes = properties.MakeReadOnly();
    }
}
