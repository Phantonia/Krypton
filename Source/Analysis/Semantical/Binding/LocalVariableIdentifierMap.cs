using Krypton.Analysis.AST.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical.Binding
{
    public sealed class LocalVariableIdentifierMap : IIdentifierMap
    {
        public LocalVariableIdentifierMap() : base() { }

        private int level = 0;
        private readonly Dictionary<string, LocalVariable> variables = new();

        public SymbolNode? this[string identifier]
        {
            get
            {
                if (variables.TryGetValue(identifier, out LocalVariable var))
                {
                    return var.variable;
                }

                return null;
            }
        }

        public bool AddSymbol(string identifier, LocalVariableSymbolNode symbol)
        {
            return variables.TryAdd(identifier, new LocalVariable(symbol, level));
        }

        public void Clear()
        {
            variables.Clear();
        }

        public void EnterBlock()
        {
            level++;
        }

        public void LeaveBlock()
        {
            Debug.Assert(level > 0);

            foreach (var kvp in variables)
            {
                if (kvp.Value.level == level)
                {
                    variables.Remove(kvp.Key);
                }
            }

            level--;
        }

        bool IIdentifierMap.AddSymbol(string identifier, SymbolNode symbol)
        {
            LocalVariableSymbolNode? var = symbol as LocalVariableSymbolNode;
            Debug.Assert(var != null);
            return AddSymbol(identifier, var);
        }

        private readonly struct LocalVariable
        {
            public LocalVariable(LocalVariableSymbolNode variable, int level)
            {
                this.level = level;
                this.variable = variable;
            }

            public readonly int level;
            public readonly LocalVariableSymbolNode variable;
        }
    }
}
