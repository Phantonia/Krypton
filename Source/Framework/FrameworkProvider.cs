using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using Krypton.Utilities;
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
                [FrameworkType.Bool] = GetBoolType(),
                [FrameworkType.Char] = GetCharType(),
            };

            List<FunctionSymbol> functions = new(capacity: 1)
            {
                new FunctionSymbol("Output", FrameworkType.None,
                    parameters: new[]
                    {
                        new ParameterSymbol("text", FrameworkType.String)
                    })
            };

            return new FrameworkVersion(minimalLanguageVersion: 0, types, functions);
        }

        private static TypeSymbol GetBoolType()
        {
            return new TypeSymbol("Bool", FrameworkType.Bool,
                binaryOperations: new[]
                {
                    MakeOperation(Operator.DoubleEquals, "===", FrameworkType.Bool),
                    MakeOperation(Operator.ExclamationEquals, "!==", FrameworkType.Bool),
                    MakeOperation(Operator.AndKeyword, "&&", FrameworkType.Bool),
                    MakeOperation(Operator.OrKeyword, "||", FrameworkType.Bool),
                    MakeOperation(Operator.XorKeyword, "^", FrameworkType.Bool)
                },
                unaryOperations: new[]
                {
                    new UnaryOperationSymbol(Operator.NotKeyword, FrameworkType.Bool, FrameworkType.Bool,
                        exp => $"!({exp})")
                });

            BinaryOperationSymbol MakeOperation(Operator op, string jsOperator, FrameworkType returnType)
            {
                return new BinaryOperationSymbol(op, FrameworkType.Bool, FrameworkType.Bool, returnType,
                    (x, y) => $"({x}){jsOperator}({y})");
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
                binaryOperations: new[]
                {
                    MakeBinaryOperator(Operator.DoubleAsterisk, "expon"),
                    MakeBinaryOperator(Operator.Asterisk, "mul"),
                    MakeBinaryOperator(Operator.ForeSlash, "rdiv"),
                    MakeBinaryOperator(Operator.Plus, "plus"),
                    MakeBinaryOperator(Operator.Minus, "minus"),
                    MakeBinaryOperator(Operator.DoubleEquals, "eq"),
                    MakeBinaryOperator(Operator.ExclamationEquals, "neq"),
                },
                unaryOperations: new[]
                {
                    new UnaryOperationSymbol(Operator.Minus, FrameworkType.Complex, FrameworkType.Complex,
                        exp => $"({exp}).$op_neg()"),
                },
                properties: new[]
                {
                    MakeProperty("Real"),
                    MakeProperty("Imaginary"),
                });

            BinaryOperationSymbol MakeBinaryOperator(Operator op, string jsMethodName, FrameworkType returnType = FrameworkType.Complex)
            {
                return new BinaryOperationSymbol(op, FrameworkType.Complex, FrameworkType.Complex, returnType,
                    (x, y) => $"({x}).$op_{jsMethodName}({y})"); // for example looks like this: z1.$op_add(z2)
            }

            PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name, FrameworkType.Rational,
                        exp => $"({exp}).$get_{name}");
            }
        }

        private static TypeSymbol GetIntType()
        {
            return new LiteralTypeSymbol<long>("Int", FrameworkType.Int,
                default(JsLiteralConversion).ConvertIntLiteral,
                binaryOperations: new[]
                {
                    MakeBinaryOperator(Operator.Asterisk, jsOperator: "*"),
                    MakeBinaryOperator(Operator.DivKeyword, generator: (x, y) => $"Math.floor(({x}) / ({y}))"),
                    MakeBinaryOperator(Operator.ModKeyword, jsOperator: "%"),
                    MakeBinaryOperator(Operator.Plus, jsOperator: "+"),
                    MakeBinaryOperator(Operator.Minus, jsOperator: "-"),
                    MakeBinaryOperator(Operator.Ampersand, jsOperator: "&"),
                    MakeBinaryOperator(Operator.Caret, jsOperator: "^"),
                    MakeBinaryOperator(Operator.Pipe, jsOperator: "|"),
                    MakeBinaryOperator(Operator.SingleRightArrow, jsOperator: ">>"),
                    MakeBinaryOperator(Operator.SingleLeftArrow, jsOperator: "<<"),
                    MakeBinaryOperator(Operator.LessThan, jsOperator: "<"),
                    MakeBinaryOperator(Operator.LessThanEquals, jsOperator: "<="),
                    MakeBinaryOperator(Operator.GreaterThanEquals, jsOperator: ">="),
                    MakeBinaryOperator(Operator.GreaterThan, jsOperator: ">"),
                    MakeBinaryOperator(Operator.DoubleEquals, jsOperator: "==="),
                    MakeBinaryOperator(Operator.ExclamationEquals, jsOperator: "!=="),
                },
                unaryOperations: new[]
                {
                    MakeUnaryOperator(Operator.Tilde, "~"),
                    MakeUnaryOperator(Operator.Minus, "-")
                },
                implicitConversions: new[]
                {
                    MakeConversion(FrameworkType.Rational, exp => $"new Rational(({exp}),1)"),
                    MakeConversion(FrameworkType.Complex, exp => $"new Complex(new Rational(({exp}),1),0)")
                });

            BinaryOperationSymbol MakeBinaryOperator(Operator op, string jsOperator = "", FrameworkType returnType = FrameworkType.Int, BinaryGenerator? generator = null)
            {
                return new BinaryOperationSymbol(op, FrameworkType.Int, FrameworkType.Int, returnType,
                    generator ?? ((x, y) => $"({x}){jsOperator}({y})"));
            }

            UnaryOperationSymbol MakeUnaryOperator(Operator op, string jsOperator)
            {
                return new UnaryOperationSymbol(op, FrameworkType.Int, FrameworkType.Int,
                    exp => $"{jsOperator}({exp})");
            }

            ImplicitConversionSymbol MakeConversion(FrameworkType returnType, UnaryGenerator generator)
            {
                return new ImplicitConversionSymbol(FrameworkType.Int, returnType, generator);
            }
        }

        private static TypeSymbol GetRationalType()
        {
            return new LiteralTypeSymbol<Rational>("Rational", FrameworkType.Rational,
                default(JsLiteralConversion).ConvertRationalLiteral,
                binaryOperations: new[]
                {
                    MakeBinaryOperator(Operator.DoubleAsterisk, "expon"),
                    MakeBinaryOperator(Operator.Asterisk, "mul"),
                    MakeBinaryOperator(Operator.ForeSlash, "rdiv"),
                    MakeBinaryOperator(Operator.ModKeyword, "mod"),
                    MakeBinaryOperator(Operator.Plus, "plus"),
                    MakeBinaryOperator(Operator.Minus, "minus"),
                    MakeBinaryOperator(Operator.LessThan, "less"),
                    MakeBinaryOperator(Operator.LessThanEquals, "leq"),
                    MakeBinaryOperator(Operator.GreaterThanEquals, "geq"),
                    MakeBinaryOperator(Operator.GreaterThan, "gre"),
                    MakeBinaryOperator(Operator.DoubleEquals, "eq"),
                    MakeBinaryOperator(Operator.ExclamationEquals, "neq"),
                },
                unaryOperations: new[]
                {
                    new UnaryOperationSymbol(Operator.Minus, FrameworkType.Rational, FrameworkType.Rational,
                        exp => $"({exp}).$op_neg()"),
                },
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

            BinaryOperationSymbol MakeBinaryOperator(Operator op, string jsMethodName, FrameworkType returnType = FrameworkType.Rational)
            {
                return new BinaryOperationSymbol(op, FrameworkType.Rational, FrameworkType.Rational, returnType,
                    (x, y) => $"({x}).$op_{jsMethodName}({y})"); // for example looks like this: r1.$op_add(r2)
            }

            PropertySymbol MakeProperty(string name)
            {
                return new PropertySymbol(name, FrameworkType.Int,
                        exp => $"({exp}).$get_{name}");
            }
        }

        private static TypeSymbol GetStringType()
        {
            PropertySymbol lengthProp = new("Length", FrameworkType.Int,
                exp => $"({exp}).length");

            BinaryOperationSymbol concatOp = new(Operator.Plus, FrameworkType.String, FrameworkType.String, FrameworkType.String,
                (x, y) => $"({x}) + ({y})");

            BinaryOperationSymbol equalsOp = new(Operator.DoubleEquals, FrameworkType.String, FrameworkType.String, FrameworkType.Bool,
                (x, y) => $"({x}) === ({y})");

            BinaryOperationSymbol notEqualsOp = new(Operator.ExclamationEquals, FrameworkType.String, FrameworkType.String, FrameworkType.Bool,
                (x, y) => $"({x}) !== ({y})");

            return new LiteralTypeSymbol<string>("String", FrameworkType.String,
                default(JsLiteralConversion).ConvertStringLiteral,
                binaryOperations: new[] { concatOp, equalsOp, notEqualsOp },
                properties: new[] { lengthProp });
        }
    }
}
