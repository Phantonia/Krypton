using Krypton.Framework;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class TypeSymbolNode : SymbolNode
    {
        private protected TypeSymbolNode(string name, int lineNumber) : base(name, lineNumber) { }

        public ReadOnlyDictionary<Operator, BinaryOperationSymbolNode> BinaryOperations { get; private set; }

        public ReadOnlyDictionary<Operator, UnaryOperationSymbolNode> UnaryOperations { get; private set; }

        internal void SetBinaryOperations(IDictionary<Operator, BinaryOperationSymbolNode> binaryOperations) => BinaryOperations = binaryOperations.MakeReadOnly();

        internal void SetUnaryOperations(IDictionary<Operator, UnaryOperationSymbolNode> unaryOperations) => UnaryOperations = unaryOperations.MakeReadOnly();
    }
}
