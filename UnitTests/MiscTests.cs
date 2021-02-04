using Krypton.Analysis.Errors;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public sealed class MiscTests
    {
        [Test]
        public void ErrorHelpersTest()
        {
            // get offending line test
            {
                const string String = @"Line 1
Line 2
Line 3";

                int column = 3;
                string line = ErrorProvider.GetOffendingLine(String, 2, ref column);

                Assert.AreEqual("Line 2", line);
                Assert.AreEqual(3, column);
                Assert.AreEqual('e', line[column]);
            }

            // get offending line with trimming test
            {
                const string String = @"Line 1
    Line 2  ";

                int column = 6;
                string line = ErrorProvider.GetOffendingLine(String, 2, ref column);

                Assert.AreEqual("Line 2", line);
                Assert.AreEqual(2, column);
                Assert.AreEqual('n', line[column]);
            }

            // get column test
            {
                const string String = @"0123
678";

                int index = 8;
                int column = ErrorProvider.GetColumn(String, index);

                Assert.AreEqual(3, column);
            }
        }
    }
}
