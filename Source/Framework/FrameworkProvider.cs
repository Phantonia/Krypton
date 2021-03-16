using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using System;
using System.Collections.Generic;

namespace Krypton.Framework
{
    // I know that this file is a mess, no need to tell me >.<
    public static class FrameworkProvider
    {
        public static FrameworkVersion GetFrameworkVersion0()
        {
            Dictionary<FrameworkType, TypeSymbol> types = new(capacity: 6)
            {
                [FrameworkType.String] = GetStringType(),
                [FrameworkType.Int] = GetIntType(),
                [FrameworkType.Rational] = GetRationalType(),
                [FrameworkType.Complex] = GetComplexType(),
                [FrameworkType.Bool] = new TypeSymbol("Bool", FrameworkType.Bool, implicitConversions: new[]
                { 
                    new ImplicitConversionSymbol(FrameworkType.Bool, FrameworkType.String, new MethodCallCodeGenerationInformation("toString"))
                }),
                [FrameworkType.Char] = GetCharType(),
            };

            List<FunctionSymbol> functions = new(capacity: 1)
            {
                new FunctionSymbol("Output",
                                   FrameworkType.None,
                                   new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.ConsoleLog),
                                   parameters: new[]
                                   {
                                       new ParameterSymbol("text", FrameworkType.String)
                                   }),
                new FunctionSymbol("Sqrt",
                                   returnType: FrameworkType.Rational,
                                   null!, // actually the code generation info is not used, so we should fix that, but not now
                                   new[]
                                   {
                                       new ParameterSymbol("number", FrameworkType.Rational)
                                   })
            };

            long denom = (long)Math.Pow(10, 18);
            List<ConstantSymbol> constants = new(capacity: 4)
            {
                new ConstantSymbol<Rational>("PI", new Rational(3141592653589793238, denom)),
                new ConstantSymbol<Rational>("E", new Rational(2718281828459045235, denom)),
                new ConstantSymbol<Rational>("TAU", new Rational(6283185307179586476, denom)),
                new ConstantSymbol<Rational>("PHI", new Rational(1618033988749894848, denom))
            };

            List<BinaryOperationSymbol> binaryOperations = GetBinaryOperations();
            List<UnaryOperationSymbol> unaryOperations = GetUnaryOperations();

            return new FrameworkVersion(minimalLanguageVersion: 0, types, functions, constants, binaryOperations, unaryOperations);
        }

        private static List<BinaryOperationSymbol> GetBinaryOperations()
        {
            return new List<BinaryOperationSymbol>(capacity: 45)
            {
                // Bool operators
                MakeOperationWithJsOperator(Operator.DoubleEquals, "===", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.ExclamationEquals, "!==", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.AndKeyword, "&&", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.OrKeyword, "||", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.XorKeyword, "^", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),

                // Complex operators
                MakeOperationWithMethod(Operator.DoubleAsterisk, "exponentiate", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Asterisk, "multiply", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.ForeSlash, "divide", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Plus, "add", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Minus, "subtract", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.DoubleEquals, "equals", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.ExclamationEquals, "notEquals", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Bool),

                // Int operators
                new BinaryOperationSymbol(Operator.DoubleAsterisk, FrameworkType.Int, FrameworkType.Int, FrameworkType.Rational,
                    new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.IntPowerInt)),
                    //(x, y) => $"(new Rational(({x}),1)).$op_expon(new Rational(({y}),1))"),
                MakeOperationWithJsOperator(Operator.Asterisk, "*", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                new BinaryOperationSymbol(Operator.DivKeyword, FrameworkType.Int, FrameworkType.Int, FrameworkType.Int,
                    new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.IntegerDivision)),
                new BinaryOperationSymbol(Operator.ForeSlash, FrameworkType.Int, FrameworkType.Int, FrameworkType.Rational,
                    new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.IntToRationalDivision)),
                MakeOperationWithJsOperator(Operator.ModKeyword, "%", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.Plus, "+", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.Minus, "-", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.Ampersand, "&", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.Caret, "^", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.Pipe, "|", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.SingleRightArrow, ">>", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.SingleLeftArrow, "<<", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                MakeOperationWithJsOperator(Operator.LessThan, "<", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.LessThanEquals, "<=", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.GreaterThanEquals, ">=", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.GreaterThan, ">", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.DoubleEquals, "===", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.ExclamationEquals, "!==", FrameworkType.Int, FrameworkType.Int, FrameworkType.Bool),

                // Rational operators
                MakeOperationWithMethod(Operator.DoubleAsterisk, "exponentiate", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Asterisk, "multiply", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.ForeSlash, "divide", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.ModKeyword, "mod", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Plus, "add", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Minus, "subtract", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.LessThan, "isLessThan", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.LessThanEquals, "isLessThanOrEquals", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.GreaterThanEquals, "isGreaterThanOrEquals", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.GreaterThan, "isGreaterThan", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.DoubleEquals, "equals", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.ExclamationEquals, "notEquals", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),

                // String operators
                MakeOperationWithJsOperator(Operator.Plus, "+", FrameworkType.String, FrameworkType.String, FrameworkType.String),
                MakeOperationWithJsOperator(Operator.DoubleEquals, "===", FrameworkType.String, FrameworkType.String, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.ExclamationEquals, "!==", FrameworkType.String, FrameworkType.String, FrameworkType.Bool),
            };

            static BinaryOperationSymbol MakeOperationWithJsOperator(Operator op,
                                                                     string jsOperator,
                                                                     FrameworkType leftType,
                                                                     FrameworkType rightType,
                                                                     FrameworkType returnType)
            {
                return new BinaryOperationSymbol(op,
                                                 leftType,
                                                 rightType,
                                                 returnType,
                                                 new JsOperatorCodeGenerationInformation(jsOperator));
            }

            static BinaryOperationSymbol MakeOperationWithMethod(Operator op,
                                                                 string jsMethodName,
                                                                 FrameworkType leftType,
                                                                 FrameworkType rightType,
                                                                 FrameworkType returnType)
            {
                // for example looks like this: z1.add(z2)
                return new BinaryOperationSymbol(op,
                                                 leftType,
                                                 rightType,
                                                 returnType,
                                                 new MethodCallCodeGenerationInformation(jsMethodName));
            }
        }

        private static TypeSymbol GetCharType()
        {
            return new TypeSymbol("Char", FrameworkType.Char,
                implicitConversions: new[]
                {
                    new ImplicitConversionSymbol(FrameworkType.Char,
                                                 FrameworkType.Int,
                                                 new IdentityCodeGenerationInformation()),
                    new ImplicitConversionSymbol(FrameworkType.Char,
                                                 FrameworkType.String,
                                                 new FunctionCallCodeGenerationInformation("String.fromCharCode")),
                });
        }

        private static TypeSymbol GetComplexType()
        {
            return new TypeSymbol("Complex", FrameworkType.Complex,
                properties: new[]
                {
                    MakeProperty("Real"),
                    MakeProperty("Imaginary"),
                },
                implicitConversions: new[]
                {
                    new ImplicitConversionSymbol(FrameworkType.Complex,
                                                 FrameworkType.String,
                                                 new MethodCallCodeGenerationInformation("toString"))
                });

            static PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name,
                                          FrameworkType.Rational,
                                          new MethodCallCodeGenerationInformation("get" + name));
            }
        }

        private static TypeSymbol GetIntType()
        {
            return new TypeSymbol("Int", FrameworkType.Int,
                implicitConversions: new[]
                {
                    MakeConversion(FrameworkType.Rational, new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.IntToRational)),
                    MakeConversion(FrameworkType.Complex, new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.IntToComplex)),
                    MakeConversion(FrameworkType.String, new MethodCallCodeGenerationInformation("toString")),
                });

            static ImplicitConversionSymbol MakeConversion(FrameworkType returnType,
                                                           CodeGenerationInformation codeGenerationInfo)
            {
                return new ImplicitConversionSymbol(FrameworkType.Int, returnType, codeGenerationInfo);
            }
        }

        private static TypeSymbol GetRationalType()
        {
            return new TypeSymbol("Rational", FrameworkType.Rational,
                implicitConversions: new[]
                {
                    new ImplicitConversionSymbol(FrameworkType.Rational,
                                                 FrameworkType.Complex,
                                                 new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.RationalToComplex)),
                    new ImplicitConversionSymbol(FrameworkType.Rational,
                                                 FrameworkType.String,
                                                 new MethodCallCodeGenerationInformation("toString")),
                },
                properties: new[]
                {
                    MakeProperty("Numerator"),
                    MakeProperty("Denominator"),
                });

            static PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name,
                                          FrameworkType.Int,
                                          new MethodCallCodeGenerationInformation("get" + name));
            }
        }

        private static TypeSymbol GetStringType()
        {
            return new TypeSymbol("String", FrameworkType.String,
                properties: new[]
                {
                    new PropertySymbol("Length",
                                       FrameworkType.Int,
                                       new SpecialCodeGenerationInformation(SpecialCodeGenerationKind.GetLengthProp)),
                });
        }

        private static List<UnaryOperationSymbol> GetUnaryOperations()
        {
            return new List<UnaryOperationSymbol>(capacity: 4)
            {
                new UnaryOperationSymbol(Operator.NotKeyword,
                                         FrameworkType.Bool,
                                         FrameworkType.Bool,
                                         new JsOperatorCodeGenerationInformation("!")),

                new UnaryOperationSymbol(Operator.Tilde,
                                         FrameworkType.Int,
                                         FrameworkType.Int,
                                         new JsOperatorCodeGenerationInformation("~")),

                new UnaryOperationSymbol(Operator.Minus,
                                         FrameworkType.Int,
                                         FrameworkType.Int,
                                         new JsOperatorCodeGenerationInformation("-")),

                new UnaryOperationSymbol(Operator.Minus,
                                         FrameworkType.Rational,
                                         FrameworkType.Rational,
                                         new MethodCallCodeGenerationInformation("negate")),
            };
        }
    }
}
