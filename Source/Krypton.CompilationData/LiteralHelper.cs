using System;
using System.Diagnostics;
using System.Numerics;

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
                      || type == typeof(BigInteger));

            // still needs Complex and Rational...
        }

        public static string LiteralToText<T>(T value)
        {
            AssertTypeIsLiteralType<T>();

            throw new NotImplementedException();
        }
    }
}
