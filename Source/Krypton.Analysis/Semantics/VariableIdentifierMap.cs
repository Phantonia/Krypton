﻿using Krypton.CompilationData.Symbols;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class VariableIdentifierMap
    {
        public VariableIdentifierMap() { }

        private int level = 0;
        private readonly Dictionary<string, Variable> variables = new();

        public VariableSymbol this[string identifier] => variables[identifier].variable;

        public bool AddSymbol(string identifier, VariableSymbol variable)
        {
            return variables.TryAdd(identifier, new Variable(variable, level));
        }

        public void EnterBlock()
        {
            level++;
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out Symbol? variable)
        {
            bool ret = TryGet(identifier, out VariableSymbol? var);
            variable = var;
            return ret;
        }

        public bool TryGet(string identifier, [NotNullWhen(true)] out VariableSymbol? variable)
        {
            if (variables.TryGetValue(identifier, out Variable var))
            {
                variable = var.variable;
                return true;
            }

            variable = null;
            return false;
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

        private readonly struct Variable
        {
            public Variable(VariableSymbol variable, int level)
            {
                this.variable = variable;
                this.level = level;
            }

            public readonly int level;
            public readonly VariableSymbol variable;
        }

        private string GetDebuggerDisplay()
        {
            return $"{GetType().Name}; Count = {variables.Count}";
        }
    }
}
