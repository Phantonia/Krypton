using Krypton.Framework.Literals;
using System;
using System.Text;

namespace Krypton.CodeGeneration
{
    internal static class LiteralGenerator
    {
        public static void EmitBoolLiteral(bool literal, StringBuilder output)
        {
            output.Append(literal ? "true" : "false");
        }

        public static void EmitCharLiteral(char literal, StringBuilder output)
        {
            output.Append((int)literal);
        }

        public static void EmitImaginaryLiteral(Rational literal, StringBuilder output)
        {
            output.Append("new Complex(new Rational(0,1),");
            EmitRationalLiteral(literal, output);
            output.Append(')');
        }

        public static void EmitIntLiteral(long literal, StringBuilder output)
        {
            output.Append(literal);
        }

        public static void EmitRationalLiteral(Rational literal, StringBuilder output)
        {
            output.Append("new Rational(");
            EmitIntLiteral(literal.Numerator, output);
            output.Append(',');
            EmitIntLiteral(literal.Denominator, output);
            output.Append(')');
        }

        public static void EmitStringLiteral(string literal, StringBuilder output)
        {
            output.Append('"');

            foreach (char c in literal)
            {
                if (char.IsDigit(c) || char.IsLetter(c))
                {
                    output.Append(c);
                }
                else
                {
                    output.Append("\\u");
                    output.Append(Convert.ToString(c, toBase: 16).PadLeft(4, '0'));
                }
            }

            output.Append('"');
        }
    }
}
