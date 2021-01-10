using NUnit.Framework;
using System;

namespace UnitTests
{
    public static class MyAssert
    {
        // For some reason, the original Assert.Throws<> method does not catch the exception.
        // This makes debugging the tests a horror, so I wrote my own method.
        public static TException? Throws<TException>(Action action)
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
    }
}
