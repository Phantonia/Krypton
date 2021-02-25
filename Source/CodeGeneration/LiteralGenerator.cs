using Krypton.Framework.Literals;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.CodeGeneration
{
    internal static class LiteralGenerator
    {
        public static string ConvertBoolLiteral(bool literal) => literal ? "true" : "false";

        public static string ConvertCharLiteral(char literal) => ((int)literal).ToString();

        public static string ConvertComplexLiteral(Complex literal) => $"new Complex({ConvertRationalLiteral(literal.Real)}, {ConvertRationalLiteral(literal.Imaginary)})";

        public static string ConvertImaginaryLiteral(Rational literal) => $"new Complex(new Rational(0, 1), {ConvertRationalLiteral(literal)})";

        public static string ConvertIntLiteral(long literal) => literal.ToString();

        public static string ConvertRationalLiteral(Rational literal) => $"new Rational({ConvertIntLiteral(literal.Numerator)}, {ConvertIntLiteral(literal.Denominator)})";

        public static string ConvertStringLiteral(string literal)
        {
            throw new NotImplementedException();
        }
    }
}
