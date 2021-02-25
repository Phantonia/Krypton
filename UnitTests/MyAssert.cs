using Krypton.Analysis;
using Krypton.Analysis.Errors;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public static class MyAssert
    {
        // For some reason, the original Assert.Throws<> method does not catch the exception.
        // This makes debugging the tests a horror, so I wrote my own method.
        public static TException Throws<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action(); // this is expected to throw

                Assert.Fail();
                return null;
            }
            catch (TException ex)
            {
                return ex;
            }
        }

        public static ErrorEventArgs Error(Action action)
        {
            ErrorEventArgs result = null;

            ErrorEventHandler handler = e =>
            {
                Assert.IsNull(result);
                result = e;
            };

            ErrorProvider.Error += handler;

            action();

            ErrorProvider.Error -= handler;

            Assert.NotNull(result);

            return result!;
        }

        public static ErrorEventArgs Error(string code)
        {
            return Error(() =>
            {
                Compilation compilation = Analyser.Analyse(code);
                Assert.IsNull(compilation);
            });
        }

        public static ErrorEventArgs Error(string code, ErrorCode errorCode)
        {
            ErrorEventArgs e = Error(code);
            Assert.AreEqual(errorCode, e.ErrorCode);
            return e;
        }

        public static T NoError<T>(Func<T> func)
        {
            ErrorEventHandler handler = e =>
            {
                Assert.Fail($"Got the error {e.ErrorCode}");
            };

            ErrorProvider.Error += handler;

            T v = func();

            ErrorProvider.Error -= handler;

            return v;
        }

        public static Compilation NoError(string code)
        {
            return NoError(() =>
            {
                Compilation compilation = Analyser.Analyse(code);
                Assert.NotNull(compilation);
                return compilation!;
            });
        }

        public static void EmittedCorrectTopLevelStatement(string expected, string actual)
        {
            Assert.AreEqual($"function $main(){{{expected}}}$main();", actual);
        }
    }
}
