using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using Krypton.Utilities;
using System;
using System.Collections.Generic;

namespace Krypton.Framework
{
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
                [FrameworkType.Bool] = new TypeSymbol("Bool", FrameworkType.Bool),
                [FrameworkType.Char] = GetCharType(),
            };
            
            List<FunctionSymbol> functions = new(capacity: 1)
            {
                new FunctionSymbol("Output", FrameworkType.None,
                    p => $"console.log({p[0]})",
                    parameters: new[]
                    {
                        new ParameterSymbol("text", FrameworkType.String)
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
            return new List<BinaryOperationSymbol>(capacity: 43)
            {
                // Bool operators
                MakeOperationWithJsOperator(Operator.DoubleEquals, "===", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.ExclamationEquals, "!==", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.AndKeyword, "&&", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.OrKeyword, "||", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),
                MakeOperationWithJsOperator(Operator.XorKeyword, "^", FrameworkType.Bool, FrameworkType.Bool, FrameworkType.Bool),

                // Complex operators
                MakeOperationWithMethod(Operator.DoubleAsterisk, "expon", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Asterisk, "mul", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.ForeSlash, "rdiv", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Plus, "plus", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.Minus, "minus", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Complex),
                MakeOperationWithMethod(Operator.DoubleEquals, "eq", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.ExclamationEquals, "neq", FrameworkType.Complex, FrameworkType.Complex, FrameworkType.Bool),

                // Int operators
                MakeOperationWithJsOperator(Operator.Asterisk, "*", FrameworkType.Int, FrameworkType.Int, FrameworkType.Int),
                new BinaryOperationSymbol(Operator.DivKeyword, FrameworkType.Int, FrameworkType.Int, FrameworkType.Int,
                    (x, y) => $"Math.floor(({x})/({y}))"),
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
                MakeOperationWithMethod(Operator.DoubleAsterisk, "expon", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Asterisk, "mul", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.ForeSlash, "rdiv", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.ModKeyword, "mod", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Plus, "plus", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.Minus, "minus", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Rational),
                MakeOperationWithMethod(Operator.LessThan, "less", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.LessThanEquals, "leq", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.GreaterThanEquals, "geq", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.GreaterThan, "gre", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.DoubleEquals, "eq", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),
                MakeOperationWithMethod(Operator.ExclamationEquals, "neq", FrameworkType.Rational, FrameworkType.Rational, FrameworkType.Bool),

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
                return new BinaryOperationSymbol(op, leftType, rightType, returnType,
                    (x, y) => $"({x}){jsOperator}({y})");
            }

            static BinaryOperationSymbol MakeOperationWithMethod(Operator op,
                                                                 string jsMethodName,
                                                                 FrameworkType leftType,
                                                                 FrameworkType rightType,
                                                                 FrameworkType returnType)
            {
                return new BinaryOperationSymbol(op, leftType, rightType, returnType,
                    (x, y) => $"({x}).$op_{jsMethodName}({y})"); // for example looks like this: z1.$op_add(z2)
            }
        }

        private static TypeSymbol GetCharType()
        {
            return new LiteralTypeSymbol<char>("Char", FrameworkType.Char,
                default(JsLiteralConversion).ConvertCharLiteral,
                implicitConversions: new[]
                {
                    new ImplicitConversionSymbol(FrameworkType.Char, FrameworkType.Int,
                        exp => exp)
                });
        }

        private static TypeSymbol GetComplexType()
        {
            return new LiteralTypeSymbol<Complex>("Complex", FrameworkType.Complex,
                default(JsLiteralConversion).ConvertComplexLiteral,
                properties: new[]
                {
                    MakeProperty("Real"),
                    MakeProperty("Imaginary"),
                });

            static PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name, FrameworkType.Rational,
                        exp => $"({exp}).$get_{name}");
            }
        }

        private static TypeSymbol GetIntType()
        {
            return new LiteralTypeSymbol<long>("Int", FrameworkType.Int,
                default(JsLiteralConversion).ConvertIntLiteral,
                implicitConversions: new[]
                {
                    MakeConversion(FrameworkType.Rational, exp => $"new Rational(({exp}),1)"),
                    MakeConversion(FrameworkType.Complex, exp => $"new Complex(new Rational(({exp}),1),0)")
                });

            static ImplicitConversionSymbol MakeConversion(FrameworkType returnType, UnaryGenerator generator)
            {
                return new ImplicitConversionSymbol(FrameworkType.Int, returnType, generator);
            }
        }

        private static TypeSymbol GetRationalType()
        {
            return new LiteralTypeSymbol<Rational>("Rational", FrameworkType.Rational,
                default(JsLiteralConversion).ConvertRationalLiteral,
                implicitConversions: new[]
                {
                    new ImplicitConversionSymbol(FrameworkType.Rational, FrameworkType.Complex,
                        exp => $"new Complex(({exp}), 0)"),
                },
                properties: new[]
                {
                    MakeProperty("Numerator"),
                    MakeProperty("Denominator"),
                });

            static PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name, FrameworkType.Int,
                    exp => $"({exp}).$get_{name}");
            }
        }

        private static TypeSymbol GetStringType()
        {
            PropertySymbol lengthProp = new("Length", FrameworkType.Int,
                exp => $"({exp}).length");

            return new LiteralTypeSymbol<string>("String", FrameworkType.String,
                default(JsLiteralConversion).ConvertStringLiteral,
                properties: new[]
                {
                    new PropertySymbol("Length", FrameworkType.Int,
                        exp => $"({exp}).length")
                });
        }

        private static List<UnaryOperationSymbol> GetUnaryOperations()
        {
            return new List<UnaryOperationSymbol>(capacity: 4)
            {
                new UnaryOperationSymbol(Operator.NotKeyword, FrameworkType.Bool, FrameworkType.Bool,
                    exp => $"!({exp})"),
                new UnaryOperationSymbol(Operator.Tilde, FrameworkType.Int, FrameworkType.Int,
                    exp => $"~({exp})"),
                new UnaryOperationSymbol(Operator.Minus, FrameworkType.Int, FrameworkType.Int,
                    exp => $"-({exp})"),
                new UnaryOperationSymbol(Operator.Minus, FrameworkType.Rational, FrameworkType.Rational,
                    exp => $"({exp}).$op_neg()"),
            };
        }
    }
}
