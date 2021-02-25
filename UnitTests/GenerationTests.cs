using Krypton.CodeGeneration;
using NUnit.Framework;

namespace UnitTests
{
    public sealed class GenerationTests
    {
        [Test]
        public void ForTest()
        {
            const string Code = @"
            For Var i = 0 While True { }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("for(let i=0;true;i++){}", o);
        }
    }
}
