using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests
{
    [SetUpFixture]
    class DealWithDebugAssert
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new AssertFailTraceListener());
        }

        private class AssertFailTraceListener : DefaultTraceListener
        {
            public AssertFailTraceListener() { }

            public override void Fail(string? message)
            {
                Assert.Fail(message);
            }

            public override void Fail(string? message, string? detailMessage)
            {
                if (!string.IsNullOrWhiteSpace(detailMessage))
                {
                    Assert.Fail(message + " Details: " + detailMessage);
                }

                Assert.Fail(message);
            }
        }
    }
}
