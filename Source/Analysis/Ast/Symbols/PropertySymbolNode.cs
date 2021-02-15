﻿using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class PropertySymbolNode : SymbolNode
    {
        public PropertySymbolNode(string identifier, TypeSymbolNode type, int lineNumber, int index) : base(identifier, lineNumber, index)
        {
            TypeNode = type;
        }

        public TypeSymbolNode TypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
