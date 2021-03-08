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
    }
}
