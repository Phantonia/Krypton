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
                      || type == typeof(long));

            // still needs Complex and Rational...
        }

        public static string LiteralToText<T>(T value)
        {
            AssertTypeIsLiteralType<T>();

            throw new NotImplementedException();
        }
    }
}
