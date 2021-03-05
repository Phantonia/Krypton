using Krypton.Analysis.Semantical;
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
                ("Var x = 3.14 + 4.9;", "let x=(new Rational(157,50)).add(new Rational(49,10));"),
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

        [Test]
        public void UnaryOperationIntNegateTest()
        {
            const string Code = @"
            Var x = -5;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let x=-(5);", o);
        }

        [Test]
        public void UnaryOperationBoolNotTest()
        {
            const string Code = @"
            Var x = Not True;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let x=!(true);", o);
        }

        [Test]
        public void UnaryOperationRationalNegateTest()
        {
            const string Code = @"
            Var x = -5.6;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let x=(new Rational(28,5)).negate();", o);
        }

        [Test]
        public void FunctionDeclarationTest()
        {
            const string Code = @"
            Func HelloWorld() { }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectFunctionDeclaration("function HelloWorld(){}", o);
        }

        [Test]
        public void FunctionDeclarationParamTest()
        {
            const string Code = @"
            Func SomeFunc(x As Int) { Var y = 4; }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectFunctionDeclaration("function SomeFunc(x){let y=4;}", o);
        }

        [Test]
        public void OutputTest()
        {
            const string Code = @"
            Var s As String;
            Output(s);
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let s;console.log(s);", o);
        }

        [Test]
        public void UdfTest()
        {
            const string Code = @"
            Func Sin(x As Rational) As Rational { }
            Sin(4);
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            Assert.AreEqual("function Sin(x){}function $main(){Sin(4);}$main();", o);
        }

        [Test]
        public void StringLiteralTest()
        {
            const string Code = @"
            Output(""xyz"");
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement(@"console.log(""xyz"");", o);
        }

        [Test]
        public void MoreComplexStringLiteralTest()
        {
            const string Code = "Output(\"xyz\\nabc$\");";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("console.log(\"xyz\\u000aabc\\u0024\");", o);
        }

        [Test]
        public void SimpleLoopLeaveTest()
        {
            const string Code = @"
            While True { Leave; }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("$loop_0:while(true){break;}", o);
        }

        [Test]
        public void NestedLoopLeaveTest()
        {
            const string Code = @"
            While True
            {
                While True
                {
                    Leave 2;
                }
            }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("$loop_0:while(true){while(true){break $loop_0;}}", o);
        }

        [Test]
        public void NestedLoopContinueTest()
        {
            const string Code = @"
            While True
            {
                While True
                {
                    Continue 2;
                }
            }
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("$loop_0:while(true){while(true){continue $loop_0;}}", o);
        }

        [Test]
        public void ConstantTest()
        {
            const string Code = @"
            Const Four = 4;
            Var x = Four;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            Assert.AreEqual("const Four=4;function $main(){let x=Four;}$main();", o);
        }

        [Test]
        public void PropertyTest()
        {
            const string Code = @"
            Var x = 3.14.Numerator;
            Var y = 4i.Real;
            Var z = ""abc"".Length;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement("let x=(new Rational(157,50)).getNumerator();" +
                                                     "let y=(new Complex(new Rational(0,1),new Rational(4,1))).getReal();" +
                                                    @"let z=(""abc"").length;", o);
        }

        [Test]
        public void ImplicitConversionTest()
        {
            const string Code = @"
            Var x As Complex = 5;
            Var y As Int = 'a';
            Var z As Rational = 4;
            Var a As Complex = 4.1;
            ";

            var c = MyAssert.NoError(Code);
            var o = CodeGenerator.GenerateCode(c, template: "");

            MyAssert.EmittedCorrectTopLevelStatement(
                $"let x=new Complex(new Rational(5,1),new Rational(0,1));" +
                $"let y={(int)'a'};" +
                $"let z=new Rational(4,1);" +
                $"let a=new Complex(new Rational(41,10),new Rational(0,1));", o);
        }
    }
}
