using Krypton.Core;
using System;
using System.Diagnostics;

namespace Krypton.CompilationData
{
    internal static class LiteralHelper
    {
        [Conditional("DEBUG")]
        public static void AssertTypeIsLiteralType<T>()
        {
            Type type = typeof(T);

            Debug.Assert(type == typeof(string)
                      || type == typeof(char)
                      || type == typeof(bool)
                      || type == typeof(long)
                      || type == typeof(Rational)
                      || type == typeof(RationalComplex));
        }
    }
}
