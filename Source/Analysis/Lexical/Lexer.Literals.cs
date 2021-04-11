using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Framework.Literals;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token LexBinaryIntegerLiteralToken() // 0b
        {
            code.Read();

            List<char> characters = new();

            while (code.Peek() != EndOfCode)
            {
                if (!((char)code.Peek()).IsBinary() && code.Peek() != '_')
                {
                    return MakeFinalToken();
                }
            }

            return MakeFinalToken();

            Token MakeFinalToken()
            {
                string literal = string.Concat(characters);
                long value = NumberLiteralParser.ParseBinary(literal);

                return new LiteralToken<long>(value, literal, lineNumber, GetTrivia());
            }
        }

        private Token LexCharLiteralToken() // '
        {
            code.Read();

            List<char> characters = new();

            bool escaped = false;

            while (code.Peek() != -1)
            {
                char c = (char)code.Read();

                switch (c)
                {
                    case '\'' when !escaped:
                        {
                            characters.Add(c);
                            string literal = string.Concat(characters);

                            if (EscapeSequences.TryParse(literal, out char value))
                            {
                                return new LiteralToken<char>(value, literal, lineNumber, GetTrivia());
                            }
                            else
                            {
                                return new InvalidToken(literal, DiagnosticsCode.UnclosedCharLiteral, lineNumber, GetTrivia()); // TODO wrong code
                            }
                        }
                    case '\\':
                        escaped = !escaped;
                        break;
                    case '\n':
                        {
                            string literal = string.Concat(characters);
                            return new InvalidToken(literal, DiagnosticsCode.UnclosedStringLiteral, lineNumber, GetTrivia());
                        }
                    default:
                        escaped = false;
                        break;

                }
            }

            {
                string literal = string.Concat(characters);
                return new InvalidToken(literal, DiagnosticsCode.UnclosedStringLiteral, lineNumber, GetTrivia());
            }
        }

        private Token LexDecimalOrRationalLiteralToken()
        {
            code.Read();

            bool alreadyHadDecimalPoint = false;

            List<char> characters = new();

            while (code.Peek() != EndOfCode)
            {
                if (code.Peek() == '_')
                {
                    if (code.)
                }
            }

            Token Finished()
            {
                string literal = string.Concat(characters);

                if (code.Peek() == 'i')
                {
                    code.Read();

                    Complex value;

                    if (NumberLiteralParser.TryParseRational(literal, out Rational rational))
                    {
                        value = new Complex(0, rational);
                    }
                }
            }
        }

        private Token LexHexadecimalIntegerLiteralToken() // 0x
        {
            code.Read();

            List<char> characters = new();

            bool? usesUpperCase = null;
            bool willBeInvalid = false;

            while (code.Peek() != -1)
            {
                bool isHex = ((char)code.Peek()).IsHex(out bool? charIsUpper);

                usesUpperCase ??= charIsUpper;

                if (charIsUpper != null && usesUpperCase != charIsUpper)
                {
                    willBeInvalid = true;
                }

                if (!isHex && code.Peek() != '_')
                {
                    return MakeFinalToken();
                }

                characters.Add((char)code.Read());
            }

            return MakeFinalToken();

            Token MakeFinalToken()
            {
                string literal = string.Concat(characters);

                if (willBeInvalid)
                {
                    return new InvalidToken("0x" + literal, DiagnosticsCode.HexLiteralWithMixedCase, lineNumber, GetTrivia());
                }
                else
                {
                    long value = NumberLiteralParser.ParseHexadecimal(literal);
                    return new LiteralToken<long>(value, "0x" + literal, lineNumber, GetTrivia());
                }
            }
        }

        private Token LexNumberLiteralToken(char firstChar)
        {
            if (firstChar == '0')
            {
                switch (code.Peek())
                {
                    case 'x':
                        code.Read();
                        return LexHexadecimalIntegerLiteralToken();
                    case 'b':
                        code.Read();
                        return LexBinaryIntegerLiteralToken();
                }
            }
        }

        private Token LexStringLiteralToken() // "
        {
            code.Read();

            List<char> characters = new();

            bool escaped = false;

            while (code.Peek() != -1)
            {
                char c = (char)code.Read();

                switch (c)
                {
                    case '"' when !escaped:
                        {
                            characters.Add(c);
                            string literal = string.Concat(characters);

                            if (StringLiteralParser.TryParse(literal, out string value))
                            {
                                return new LiteralToken<string>(value, literal, lineNumber, GetTrivia());
                            }
                            else
                            {
                                return new InvalidToken(literal, DiagnosticsCode.UnclosedStringLiteral, lineNumber, GetTrivia()); // TODO wrong code
                            }
                        }
                    case '\\':
                        escaped = !escaped;
                        break;
                    case '\n':
                        {
                            string literal = string.Concat(characters);
                            return new InvalidToken(literal, DiagnosticsCode.UnclosedStringLiteral, lineNumber, GetTrivia());
                        }
                    default:
                        escaped = false;
                        break;

                }
            }

            {
                string literal = string.Concat(characters);
                return new InvalidToken(literal, DiagnosticsCode.UnclosedStringLiteral, lineNumber, GetTrivia());
            }
        }
    }
}
