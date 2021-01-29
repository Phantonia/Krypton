using Krypton.Framework.Literals;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    public abstract class ConstantSymbolNode : SymbolNode
    {
        internal ConstantSymbolNode(string name, int lineNumber) : base(name, lineNumber) { }
    }

    public sealed class ConstantSymbolNode<T> : ConstantSymbolNode
    {
        internal ConstantSymbolNode(string name, T value, int lineNumber) : base(name, lineNumber)
        {
            AssertTypeIsCorrect();
            Value = value;
        }

        public T Value { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }

        [Conditional("DEBUG")]
        private static void AssertTypeIsCorrect()
        {
            Type type = typeof(T);
            Debug.Assert(type == typeof(long)
                      || type == typeof(Rational)
                      || type == typeof(Complex)
                      || type == typeof(string)
                      || type == typeof(char)
                      || type == typeof(bool));
        }
    }
}
