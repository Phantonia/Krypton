﻿namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class SymbolNode : Node
    {
        protected private SymbolNode(string name, int lineNumber) : base(lineNumber)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
