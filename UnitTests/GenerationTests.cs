using Krypton.CodeGeneration;
using NUnit.Framework;
using System.Linq;

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

        [Test]
        public void OperationTests()
        {
            (string input, string output)[] cases =
            {
                ("Var x = 5 + 4;", "let x=(5)+(4);"),
                ("Var x = 9 Div 4;", "let x=Math.floor((9)/(4));"),
                ("Var x = 9 ** 4;", "let x=new Rational(9,1).exponentiate(new Rational(4,1);"),
                ("Var x = 3 / 4;", "let x=new Rational(3,4);"),
                ("Var x = 3.14 + 4.9;", "let x=(new Rational(157, 50)).add(new Rational(49, 10));"),
            };

            foreach (var (expected, actual) in from c in cases
                                               let comp = MyAssert.NoError(c.input)
                                               let outp = CodeGenerator.GenerateCode(comp, template: "")
                                               select (c.output, outp))
            {
                MyAssert.EmittedCorrectTopLevelStatement(expected, actual);
            }
        }

        [Test]
        public void IfTest()
        {
            const string Code = @"
            If 4 == 4
            {

            }
            Else If 5 == 5
            {

            }
            Else
            {

            }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("if((4)===(4)){}else if((5)===(5)){}else{}", o);
        }

        [Test]
        public void ReturnTest()
        {
            const string Code = @"
            Return;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("return;", o);
        }

        [Test]
        public void AssignmentTest()
        {
            const string Code = @"
            Var x = 5;
            x = 4;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let x=5;x=4;", o);
        }

        [Test]
        public void WhileTest()
        {
            const string Code = @"
            While True { Var x = 4; }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("while(true){let x=4;}", o);
        }
    }
}
