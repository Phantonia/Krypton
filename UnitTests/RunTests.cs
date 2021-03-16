using Krypton.Compiler;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    class RunTests
    {
        const string TemplatePath = @"E:\Documents\Projects\Software\Krypton\KryptonV5\Source\Compiler\bin\Debug\net5.0\template.js";
        private readonly string template = File.ReadAllText(TemplatePath);

        private void AssertCorrectOutput(string code, string expectedOutput)
        {
            var comp = MyAssert.NoError(code);
            RunProvider runProvider = new(comp, template);
            string output = runProvider.RunAndGetOutputAsync().GetAwaiter().GetResult().Trim();
            Assert.AreEqual(output, expectedOutput);
        }

        [Test]
        public void HelloWorldTest()
        {
            const string Code = @"Output(""Hello world"");";
            AssertCorrectOutput(Code, "Hello world");
        }

        [Test]
        public void IntOutputTest()
        {
            const string Code = @"Output(4);";
            AssertCorrectOutput(Code, "4");
        }

        [Test]
        public void RationalOutputTest()
        {
            const string Code = @"Output(0.1);";
            AssertCorrectOutput(Code, "0.1");
        }

        [Test]
        public void CharOutputTest()
        {
            const string Code = @"Output('a');";
            AssertCorrectOutput(Code, "a");
        }

        [Test]
        public void PrimeTest()
        {
            const string Code = @"
            Func IsPrime(n As Int) As Bool
            {
                If n == 1
                {
                    Return False;
                }

                If n Mod 2 == 0
                {
                    Return n == 2;
                }

                For Var i = 3 While i <= Sqrt(n) With i = i + 2
                {
                    If n Mod i == 0
                    {
                        Return False;
                    }
                }

                Return True;
            }

            For Var i = 1 While i < 50
            {
                If IsPrime(i)
                {
                    Output(i);
                }
            }
            ";

            const string Expected = @"2
3
5
7
11
13
17
19
23
29
31
37
41
43
47";

            AssertCorrectOutput(Code, Expected);
        }
    }
}
