using Krypton.Framework.Literals;

namespace Krypton.Utilities
{
    internal interface ILiteralConversion
    {
        string ConvertBoolLiteral(bool literal);

        string ConvertCharLiteral(char literal);

        string ConvertComplexLiteral(Complex literal);

        string ConvertIntLiteral(long literal);

        string ConvertRationalLiteral(Rational literal);

        string ConvertStringLiteral(string literal);
    }
}
