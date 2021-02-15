using Krypton.Framework;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class TypeSymbolNode : SymbolNode
    {
        private protected TypeSymbolNode(string name, int lineNumber, int index) : base(name, lineNumber, index) { }

        public ReadOnlyDictionary<Operator, BinaryOperationSymbolNode> BinaryOperationNodes { get; private set; }

        public ReadOnlyDictionary<string, PropertySymbolNode> PropertyNodes { get; private set; }

        public ReadOnlyDictionary<Operator, UnaryOperationSymbolNode> UnaryOperationNodes { get; private set; }

        internal void SetBinaryOperations(IDictionary<Operator, BinaryOperationSymbolNode> binaryOperations)
            => BinaryOperationNodes = binaryOperations.MakeReadOnly();

        internal void SetProperties(IDictionary<string, PropertySymbolNode> properties)
            => PropertyNodes = properties.MakeReadOnly();

        internal void SetUnaryOperations(IDictionary<Operator, UnaryOperationSymbolNode> unaryOperations)
            => UnaryOperationNodes = unaryOperations.MakeReadOnly();
    }
}
