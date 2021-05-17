using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Krypton.Tests
{
    public static class AssertionHelpers
    {
        // Add new overloads whenever it is needed
        public static void AssertIsType<T0>(this object item)
        {
            var o0 = item;
            Assert.IsInstanceOfType(o0, typeof(T0));
        }

        public static void AssertAreTypes<T0, T1>(this (object, object) item)
        {
            var (o0, o1) = item;
            Assert.IsInstanceOfType(o0, typeof(T0));
            Assert.IsInstanceOfType(o1, typeof(T1));
        }

        public static void AssertAreTypes<T0, T1, T2>(this (object, object, object) item)
        {
            var (o0, o1, o2) = item;
            Assert.IsInstanceOfType(o0, typeof(T0));
            Assert.IsInstanceOfType(o1, typeof(T1));
            Assert.IsInstanceOfType(o2, typeof(T2));
        }

        public static object AssertIsType<T>(this object obj, Func<T, object> selector)
            where T : class
        {
            T? t = obj as T;
            Assert.IsNotNull(t);
            return selector(t!);
        }

        public static (object, object) AssertIsType<T>(this object obj, Func<T, (object, object)> selector)
            where T : class
        {
            T? t = obj as T;
            Assert.IsNotNull(t);
            return selector(t!);
        }

        public static (object, object) AssertAreTypes<T1, T2>(this (object, object) tuple, Func<T1, object> selector1, Func<T2, object> selector2)
            where T1 : class
            where T2 : class
        {
            (object o1, object o2) = tuple;

            T1? t1 = o1 as T1;

            Assert.IsNotNull(t1);

            T2? t2 = o2 as T2;

            Assert.IsNotNull(t2);

            return (selector1(t1!), selector2(t2!));
        }
    }

    //[TestClass]
    //public sealed class AssertionHelpersGenerator
    //{
    //    // this is not really a test, but we just use the infrastructure to generate
    //    [TestMethod] 
    //    public void GenerateAssertionHelpers()
    //    {
    //        const int MaxElementNumber = 3;

    //        StringBuilder sb = new();

    //        // generate simple checks
    //        for (int i = 1; i <= MaxElementNumber; i++)
    //        {
    //            sb.Append("public static void ");
    //            sb.Append(i == 1 ? "AssertIsType" : "AssertAreTypes");
    //            sb.Append('<');

    //            for (int j = 0; j < i - 1; j++)
    //            {
    //                sb.Append('T');
    //                sb.Append(j);
    //                sb.Append(", ");
    //            }

    //            sb.Append('T');
    //            sb.Append(i - 1);

    //            sb.Append('>');
    //            sb.Append("(this ");

    //            if (i != 1)
    //            {
    //                sb.Append('(');
    //            }

    //            for (int j = 0; j < i - 1; j++)
    //            {
    //                sb.Append("object, ");
    //            }

    //            sb.Append("object");

    //            if (i != 1)
    //            {
    //                sb.Append(')');
    //            }

    //            sb.AppendLine(" item)");

    //            sb.AppendLine("{");

    //            if (i != 1)
    //            {
    //                sb.Append("var (");

    //                for (int j = 0; j < i - 1; j++)
    //                {
    //                    sb.Append('o');
    //                    sb.Append(j);
    //                    sb.Append(", ");
    //                }

    //                sb.Append('o');
    //                sb.Append(i - 1);
    //                sb.AppendLine(") = item;");
    //            }
    //            else
    //            {
    //                sb.AppendLine("var o0 = item;");
    //            }

    //            for (int j = 0; j < i; j++)
    //            {
    //                sb.Append("Assert.IsInstanceOfType(o");
    //                sb.Append(j);
    //                sb.Append(", typeof(T");
    //                sb.Append(j);
    //                sb.Append("));");
    //                sb.AppendLine();
    //            }

    //            sb.AppendLine("}");
    //            sb.AppendLine();
    //        }

    //        for (int i = 1; i <= MaxElementNumber; i++)
    //        {
    //            string tupleType = "(" + string.Join(", ", Enumerable.Repeat("object", i)) + ")";

    //            if (i == 1)
    //            {
    //                tupleType = tupleType[1..^1];
    //            }

    //            string genericClause = "<" + string.Join(", ", Enumerable.Repeat("T", i).Select((s, i) => $"{s}{i}")) + ">";

    //            sb.Append("public static ")
    //        }

    //        Console.WriteLine(sb);
    //    }
    //}
}
