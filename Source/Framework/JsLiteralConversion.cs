using Krypton.Framework.Literals;
using System;

namespace Krypton.Utilities
{
    internal readonly struct JsLiteralConversion : ILiteralConversion
    {
        public string ConvertBoolLiteral(bool literal) => literal ? "true" : "false";

        public string ConvertCharLiteral(char literal) => ConvertStringLiteral(((int)literal).ToString());

        public string ConvertComplexLiteral(Complex literal) => $"new Complex({ConvertRationalLiteral(literal.Real)}, {ConvertRationalLiteral(literal.Imaginary)})";

        public string ConvertIntLiteral(long literal) => literal.ToString();

        public string ConvertRationalLiteral(Rational literal) => $"new Rational({ConvertIntLiteral(literal.Numerator)}, {ConvertIntLiteral(literal.Denominator)})";

        public string ConvertStringLiteral(string literal)
        {
            throw new NotImplementedException();
        }
    }
}
