﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers
{
    public sealed class BoundIdentifierNode : IdentifierNode
    {
        public BoundIdentifierNode(string identifier, SymbolNode symbol, int lineNumber) : base(identifier, lineNumber)
        {
            Symbol = symbol;
        }

        public SymbolNode Symbol { get; }

        public override BoundIdentifierNode Clone() => new(Identifier, Symbol.Clone(), LineNumber) { Parent = this };

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            // It's important that we don't populate with Symbol.
            // In the best case we repeat stuff in the enumeration;
            // in the worst case (recursive definitions) we StackOverflow.
        }
    }
}
